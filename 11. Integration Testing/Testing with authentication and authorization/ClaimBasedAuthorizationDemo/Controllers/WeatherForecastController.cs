using System.Security.Claims;
using ClaimBasedAuthorizationDemo.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;


namespace ClaimBasedAuthorizationDemo.Controllers;

[Authorize]
[ApiController]
[Route("[controller]")]
public class WeatherForecastController : ControllerBase
{
    private static readonly string[] Summaries = new[]
    {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };

    private readonly ILogger<WeatherForecastController> _logger;

    public WeatherForecastController(ILogger<WeatherForecastController> logger)
    {
        _logger = logger;
    }

    [HttpGet(Name = "GetWeatherForecast")]
    public IEnumerable<WeatherForecast> Get()
    {
        return Enumerable.Range(1, 5).Select(index => new WeatherForecast
        {
            Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
            TemperatureC = Random.Shared.Next(-20, 55),
            Summary = Summaries[Random.Shared.Next(Summaries.Length)]
        })
        .ToArray();
    }

    [AllowAnonymous]
    [HttpGet("anonymous")]
    public IEnumerable<WeatherForecast> GetAnonymous()
    {
        return Enumerable.Range(1, 5).Select(index => new WeatherForecast
        {
            Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
            TemperatureC = Random.Shared.Next(-20, 55),
            Summary = Summaries[Random.Shared.Next(Summaries.Length)]
        })
        .ToArray();
    }
    // user must have both AccessNumber(but not specific number) claims to access the resource.
    [Authorize(Policy = AppAuthorizationPolicies.RequireDrivingLicenseNumber)]
    [HttpGet("driving-license")]
    public IActionResult GetDrivingLicense()
    {
        var drivingLicenseNumber = User.Claims.FirstOrDefault(c => c.Type == AppClaimTypes.DrivingLicenseNumber)?.Value;
        return Ok(new { drivingLicenseNumber });
    }
    // user must have specific country(Nepal) claims to access the resource.
    [Authorize(Policy = AppAuthorizationPolicies.RequireCountry)]
    [HttpGet("country")]
    public IActionResult GetCountry()
    {
        var country = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Country)?.Value;
        return Ok(new { country });
    }

// user must have both the DrivingLicenseNumber and AccessNumber claims to access the resource.
    [Authorize(Policy = AppAuthorizationPolicies.RequireDrivingLicenseNumber)]
    [Authorize(Policy = AppAuthorizationPolicies.RequireAccessNumber)]
    [HttpGet("driving-license-and-access-number")]
    public IActionResult GetDrivingLicenseAndAccessNumber()
    {
        var drivingLicenseNumber = User.Claims.FirstOrDefault(c => 
            c.Type == AppClaimTypes.DrivingLicenseNumber)?.Value;
        var accessNumber = User.Claims.FirstOrDefault(c => 
            c.Type == AppClaimTypes.AccessNumber)?.Value;
        return Ok(new { drivingLicenseNumber, accessNumber });
    }

    // using RequireAssertion() method
    [Authorize(Policy = AppAuthorizationPolicies.RequireDrivingLicenseNumberAndAccessNumber)]
    [HttpGet("driving-license-and-access-number-with-requireAssertion")]
    public IActionResult GetDrivingLicenseAndAccessNumberWithRequireAssertion()
    {
        var drivingLicenseNumber = User.Claims.FirstOrDefault(c => 
            c.Type == AppClaimTypes.DrivingLicenseNumber)?.Value;
        var accessNumber = User.Claims.FirstOrDefault(c => 
            c.Type == AppClaimTypes.AccessNumber)?.Value;
        return Ok(new { drivingLicenseNumber, accessNumber });
    }
}
