using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using PolicyBasedAuthorization.Authentication;
using PolicyBasedAuthorizationService.Authentication;

namespace AuthenticationDemo.Controller;

[ApiController]
[Route("[controller]")]
public class AccountController(UserManager<AppUser> userManager, IConfiguration configuration) 
: ControllerBase
{
    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] AddOrUpdateAppUserModel model)
    {
        if (ModelState.IsValid)
        {
            var existedUser = await userManager.FindByNameAsync(model.UserName);
            if(existedUser != null)
            {
                ModelState.AddModelError("", "User name is already taken.");
                return BadRequest(ModelState);
            }
            // Create a new user object
            var user = new AppUser()
            {
                UserName = model.UserName,
                Email = model.Email,
                SecurityStamp = Guid.NewGuid().ToString()
            };
            // Try to save the user
            var result = await userManager.CreateAsync(user, model.Password);
            // If the user is successfully created, return Ok
            if (result.Succeeded)
            {
                // var token = GenerateToken(model.UserName);
                // return Ok(new { token });
                return Created();
            }
            // If there are any errors, add them to the ModelState object
            // and return the error to the client
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error.Description);
            }
        }
        // If we got this far, something failed, redisplay form
        return BadRequest(ModelState);
    }

    private string? GenerateToken(string userName, string country)
    {
        var secret = configuration["JwtConfig:Secret"];
        var issuer = configuration["JwtConfig:ValidIssuer"];
        var audience = configuration["JwtConfig:ValidAudiences"];
        if (secret is null || issuer is null || audience is null)
        {
            throw new ApplicationException("Jwt is not set in the configuration");
        }
        var signingKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret));
        var tokenHandler = new JwtSecurityTokenHandler();

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new[]
            {
                new Claim(ClaimTypes.Name, userName),
                // passing the value of AppClaimTypes.Subscription to the token
                new Claim(AppClaimTypes.Subscription, "Premium"), 
                // also added a new claim ClaimTypes.Country to the token.
                new Claim(ClaimTypes.Country, country) 
            }),
            Expires = DateTime.UtcNow.AddDays(1),
            Issuer = issuer,
            Audience = audience,
            SigningCredentials = new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256Signature)
        };

        var securityToken = tokenHandler.CreateToken(tokenDescriptor);
        var token = tokenHandler.WriteToken(securityToken);
        return token;
    }

    // Create a Login action to validate the user credentials and generate the JWT token
    [HttpPost("login-nepal")]
    public async Task<IActionResult> LoginNepal([FromBody] LoginModel model)
    {
        // Get the secret in the configuration

        // Check if the model is valid
        if (ModelState.IsValid)
        {
            var user = await userManager.FindByNameAsync(model.UserName);
            if (user != null)
            {
                if (await userManager.CheckPasswordAsync(user, model.Password))
                {
                    var token = GenerateToken(model.UserName, "Nepal");
                    return Ok(new { token });
                }
            }
            // If the user is not found, display an error message
            ModelState.AddModelError("", "Invalid username or password");
        }
        return BadRequest(ModelState);

    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginModel model)
    {
    // Get the secret in the configuration
    // Check if the model is valid
        if (ModelState.IsValid)
        {
            var user = await userManager.FindByNameAsync(model.UserName);
            if (user != null)
            {
                if (await userManager.CheckPasswordAsync(user, model.Password))
                {
                    var token = GenerateToken(model.UserName, "China");
                    return Ok(new { token });
                }
            }
        // If the user is not found, display an error message
        ModelState.AddModelError("", "Invalid username or password");
        }
        return BadRequest(ModelState);
    }

}