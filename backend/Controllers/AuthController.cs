using EcommerceApp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace EcommerceApp.Controllers
{
  [ApiController]
  [Route("api/[controller]")]
  public class AuthController : Controller
  {
    // Add methods for authentication logic here
    // e.g. Login, Register, etc.

    private readonly SignInManager<ApplicationUser> _signInManager;
    private readonly UserManager<ApplicationUser> _userManager;
    public AuthController(SignInManager<ApplicationUser> signInManager, UserManager<ApplicationUser> userManager)
    {
      _signInManager = signInManager;
      _userManager = userManager;
    }

    [HttpPost]
    [Route("login")]
    [AllowAnonymous]
    public async Task<IActionResult> Login([FromBody] LoginRequest request)
    {
      Console.WriteLine($"Login attempt: Email={request.Email}, Password={request.Password}");


      var result = await _signInManager.PasswordSignInAsync(request.Email, request.Password, isPersistent: false, lockoutOnFailure: false);

      // Check if the login was successful
      if (result.Succeeded)
      {
        // Set the authentication cookie
        await _signInManager.SignInAsync(await _userManager.FindByEmailAsync(request.Email), isPersistent: false);
        return Ok();
      }

      // Log the login attempt
      //Console.WriteLine($"Login attempt: Email={request.Email}, Password={request.Password}");

      // Implement your login logic here
      // e.g. validate credentials, generate token, etc.

      // For now, let's just return a success response
      return Unauthorized();

    }

    public class LoginRequest
    {
      public string Email { get; set; }
      public string Password { get; set; }
    }
  }



}
