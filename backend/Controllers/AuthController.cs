using EcommerceApp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using NuGet.Protocol;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

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

    [HttpPost]
    [Route("login")]
    [AllowAnonymous]

    public async Task<IActionResult> Login([FromBody] LoginRequest request)
    {
      Console.WriteLine($"Login attempt: Email={request.Email}, Password={request.Password}");

      var result = await _signInManager.PasswordSignInAsync(request.Email, request.Password, isPersistent: false, lockoutOnFailure: false);

      if (result.Succeeded)
      {
        var user = await _userManager.FindByEmailAsync(request.Email);
        var userRoles = await _userManager.GetRolesAsync(user);
        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, user.Email),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new Claim(ClaimTypes.Role, userRoles.FirstOrDefault()),
        };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        var expires = DateTime.Now.AddDays(Convert.ToDouble(_configuration["Jwt:ExpireDays"]));

        var token = new JwtSecurityToken(
            _configuration["Jwt:Issuer"],
            _configuration["Jwt:Audience"],
            claims,
            expires: expires,
            signingCredentials: creds
        );

        Console.WriteLine("token: " + token);

        var cookieOptions = new CookieOptions
        {
          HttpOnly = true,
          SameSite = SameSiteMode.Strict, //None 
          Expires = expires,
          Secure = false,
        };

        Console.WriteLine("cookieoptions: " + cookieOptions);

        Response.Cookies.Append("jwt", new JwtSecurityTokenHandler().WriteToken(token), cookieOptions);

        Response.Cookies.Append("testing", "Hejsan hoppsan!", new CookieOptions
        {
          HttpOnly = false,
          IsEssential = true,
          SameSite = SameSiteMode.Strict, //None 
          Expires = expires,
          Secure = false,
          Domain = "localhost",
        }
         );

        HttpContext.Response.Cookies.Append(
                     "test2", "value",
                     new CookieOptions() { SameSite = SameSiteMode.Lax });

        // Check the cookies
        var setCookies = Response.Headers["Set-Cookie"];
        Console.WriteLine("Set-Cookie headers:");
        foreach (var cookie in setCookies)
        {
          Console.WriteLine("the cookie:" + cookie);
        }


        //var cookies = Response.Cookies.ToJson();
        //Console.WriteLine("Cookies: " + cookies);

        return Ok();

        /*
        if (user != null)
        {
          await _signInManager.SignInAsync(user, isPersistent: false);
          return Ok();
        }
        else
        {
          return Unauthorized();
        }
        */
      }
      else
      {
        return Unauthorized();
      }

    }

    [HttpGet]
    [Route("check-auth")]
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
          tokenHandler.ValidateToken(jwtCookie, new TokenValidationParameters
          {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(key),
            ValidateIssuer = true,
            ValidIssuer = _configuration["Jwt:Issuer"],
            ValidateAudience = true,
            ValidAudience = _configuration["Jwt:Audience"],
            ValidateLifetime = true,
            ClockSkew = TimeSpan.Zero
          }, out SecurityToken validatedToken);

          return Ok();
        }
        catch (Exception ex)
        {
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
