using EcommerceApp.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace EcommerceApp.Controllers
{
  [ApiController]
  [Route("api/[controller]")]
  public class AuthController : Controller
  {
    private readonly SignInManager<ApplicationUser> _signInManager;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IConfiguration _configuration;

    public AuthController(SignInManager<ApplicationUser> signInManager, UserManager<ApplicationUser> userManager, IConfiguration configuration)
    {
      _signInManager = signInManager;
      _userManager = userManager;
      _configuration = configuration;
    }

    public async Task<JwtSecurityToken> GenerateJwtToken(string email, DateTime expires)
    {
      var user = await _userManager.FindByEmailAsync(email);
      var userRoles = await _userManager.GetRolesAsync(user);
      var claims = new[]
      {
            new Claim(JwtRegisteredClaimNames.Sub, user.Email),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new Claim(ClaimTypes.Role, userRoles.FirstOrDefault()),
        };

      var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
      var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

      var token = new JwtSecurityToken(
          _configuration["Jwt:Issuer"],
          _configuration["Jwt:Audience"],
          claims,
          expires: expires,
          signingCredentials: creds
      );

      Console.WriteLine("token: " + token);
      return token;
    }

    [HttpPost]
    [Route("login")]
    [AllowAnonymous]
    public async Task<IActionResult> Login([FromBody] LoginRequest request)
    {
      Console.WriteLine($"Login attempt: Email={request.Email}, Password={request.Password}");

      var result = await _signInManager.PasswordSignInAsync(request.Email, request.Password, isPersistent: false, lockoutOnFailure: false);

      if (result.Succeeded)
      {
        DateTime expires = DateTime.Now.AddDays(Convert.ToDouble(_configuration["Jwt:ExpireDays"]));
        var token = await GenerateJwtToken(request.Email, expires);

        var cookieOptions = new CookieOptions
        {
          HttpOnly = true,
          SameSite = SameSiteMode.Strict, //None 
          Expires = expires,
          Secure = false,
        };

        Response.Cookies.Append("jwt", new JwtSecurityTokenHandler().WriteToken(token), cookieOptions);
        Response.Cookies.Append("rox", "Testingtesting!", cookieOptions);

        return Ok();

      }
      else
      {
        return Unauthorized();
      }

    }

    [HttpGet]
    [Route("check-auth")]
    //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public IActionResult CheckAuth()
    {
      // Check if the user is authenticated by validating the JWT cookie
      var jwtCookie = Request.Cookies["jwt"];

      if (!string.IsNullOrEmpty(jwtCookie))
      {
        Console.WriteLine("JWT cookie found: " + jwtCookie);

        try
        {
          var tokenHandler = new JwtSecurityTokenHandler();
          var key = Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]);

          var validationParameters = new TokenValidationParameters
          {
            ValidateIssuerSigningKey = false,
            IssuerSigningKey = new SymmetricSecurityKey(key),
            ValidateIssuer = true,
            ValidIssuer = _configuration["Jwt:Issuer"],
            ValidateAudience = true,
            ValidAudience = _configuration["Jwt:Audience"],
            ValidateLifetime = false, // skip this for now
            ClockSkew = TimeSpan.Zero
          };

          var principal = tokenHandler.ValidateToken(jwtCookie, validationParameters, out SecurityToken validatedToken);
          //var claims = principal.Claims.Select(claim => new { claim.Type, claim.Value }).ToList();
          var claims = principal.Claims.ToDictionary(c => c.Type, c => c.Value);

          var simplifiedClaims = new Dictionary<string, string>
            {
                { "Email", claims[ClaimTypes.NameIdentifier] },
                { "Role", claims[ClaimTypes.Role] },
                { "Jti", claims[JwtRegisteredClaimNames.Jti] },
                { "Exp", claims[JwtRegisteredClaimNames.Exp] },
                { "Issuer", claims[JwtRegisteredClaimNames.Iss] },
                { "Audience", claims[JwtRegisteredClaimNames.Aud] }
            };

          Console.WriteLine("Token validated");
          return Ok(new { Claims = simplifiedClaims });
        }
        catch (Exception ex)
        {
          Console.WriteLine("Invalid token", ex.Data);
          // Handle token validation error
          return Unauthorized();
        }
      }

      Console.WriteLine("NO JWT cookie found");
      return Unauthorized();
    }

    public class LoginRequest
    {
      public string Email { get; set; }
      public string Password { get; set; }
    }
  }
}
