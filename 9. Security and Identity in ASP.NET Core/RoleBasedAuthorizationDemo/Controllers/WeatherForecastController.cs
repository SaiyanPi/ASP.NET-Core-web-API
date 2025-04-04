using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RoleBasedAuthorizationDemo.Authentication;

namespace RoleBasedAuthorizationDemo.Controllers;

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

    // [HttpGet(Name = "GetWeatherForecast")]
    // // This means that the user must have at least one of the roles
    // [Authorize(Roles = $"{AppRoles.User},{AppRoles.VipUser},{AppRoles.Administrator}")]
    // public IEnumerable<WeatherForecast> Get()
    // {
    //     return Enumerable.Range(1, 5).Select(index => new WeatherForecast
    //     {
    //         Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
    //         TemperatureC = Random.Shared.Next(-20, 55),
    //         Summary = Summaries[Random.Shared.Next(Summaries.Length)]
    //     })
    //     .ToArray();
    // }    

    // [HttpGet("vip", Name = "GetVipWeatherForecast")]
    // // This means that the user must have both roles
    // [Authorize(Roles = AppRoles.User)]
    // [Authorize(Roles = AppRoles.VipUser)]
    // public IEnumerable<WeatherForecast> GetVip()
    // {
    //     return Enumerable.Range(1, 5).Select(index => new WeatherForecast
    //     {
    //         Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
    //         TemperatureC = Random.Shared.Next(-20, 55),
    //         Summary = Summaries[Random.Shared.Next(Summaries.Length)]
    //     })
    //     .ToArray();
    // }

    // [HttpGet("admin", Name = "GetAdminWeatherForecast")]
    // [Authorize(Roles = AppRoles.Administrator)]
    // public IEnumerable<WeatherForecast> GetAdmin()
    // {
    //     return Enumerable.Range(1, 5).Select(index => new WeatherForecast
    //     {
    //         Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
    //         TemperatureC = Random.Shared.Next(-20, 55),
    //         Summary = Summaries[Random.Shared.Next(Summaries.Length)]
    //     })
    //     .ToArray();
    // }

    // policy to specify the roles that are allowed to access the API
    [HttpGet("admin-with-policy", Name = "GetAdminWeatherForecastWithPolicy")]
    [Authorize(Policy = "RequireAdministratorRole")]
    public IEnumerable<WeatherForecast> GetAdminWithPolicy()
    {
        return Enumerable.Range(1, 20).Select(index => new WeatherForecast
        {
            Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
            TemperatureC = Random.Shared.Next(-20, 55),
            Summary = Summaries[Random.Shared.Next(Summaries.Length)]
        })
        .ToArray();
    }

    [HttpGet("user-with-policy", Name = "GetUserWeatherForecastWithPolicy")]
    [Authorize(Policy = "RequireUserRole")]
    public IEnumerable<WeatherForecast> GetUserWithPolicy()
    {
        return Enumerable.Range(1, 20).Select(index => new WeatherForecast
        {
            Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
            TemperatureC = Random.Shared.Next(-20, 55),
            Summary = Summaries[Random.Shared.Next(Summaries.Length)]
        })
        .ToArray();
    }

    [HttpGet("vipUser-with-policy", Name = "GetVipUserWeatherForecastWithPolicy")]
    [Authorize(Policy = "RequireVipUserRole")]
    public IEnumerable<WeatherForecast> GetVipUserWithPolicy()
    {
        return Enumerable.Range(1, 20).Select(index => new WeatherForecast
        {
            Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
            TemperatureC = Random.Shared.Next(-20, 55),
            Summary = Summaries[Random.Shared.Next(Summaries.Length)]
        })
        .ToArray();
    }

    [HttpGet("vipUser-and-User-with-policy", Name = "GetVipUserAndUserWeatherForecastWithPolicy")]
    [Authorize(Policy = "RequireVipUserRole")]
    [Authorize(Policy = "RequireUserRole")]
    public IEnumerable<WeatherForecast> GetVipUserAndUserWithPolicy()
    {
        return Enumerable.Range(1, 20).Select(index => new WeatherForecast
        {
            Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
            TemperatureC = Random.Shared.Next(-20, 55),
            Summary = Summaries[Random.Shared.Next(Summaries.Length)]
        })
        .ToArray();
    }

    [HttpGet("userOrVipUser-with-policy", Name = "GetUserOrVipUserWeatherForecastWithPolicy")]
    [Authorize(Policy = "RequireUserRoleOrVipUserRole")]
    public IEnumerable<WeatherForecast> GetUserOrVipUserWithPolicy()
    {
        return Enumerable.Range(1, 20).Select(index => new WeatherForecast
        {
            Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
            TemperatureC = Random.Shared.Next(-20, 55),
            Summary = Summaries[Random.Shared.Next(Summaries.Length)]
        })
        .ToArray();
    }
}
