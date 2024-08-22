using Microsoft.AspNetCore.Mvc;
using MiddlewareDemo.Services;

namespace MiddlewareDemo.Controllers;

[ApiController]
[Route("[controller]")]
public class WeatherForecastController : ControllerBase
{
    private static readonly string[] Summaries = new[]
    {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };

    private readonly ILogger<WeatherForecastController> _logger;
    private readonly IStudentService _studentService;
    private readonly IAddressService _addressService;

    public WeatherForecastController(ILogger<WeatherForecastController> logger, 
    IStudentService studentService, IAddressService addressService)
    {
        _logger = logger;
        _studentService = studentService;
        _addressService = addressService;
    
    }

    [HttpGet(Name = "GetWeatherForecast")]
    public IEnumerable<WeatherForecast> Get()
    {
        // Get the "X-Correlation-Id" header from the request
        var correlationId = Request.Headers["X-CorrelationId"].FirstOrDefault();
        // Log the correlation ID
        _logger.LogInformation("Handling the request. CorrelationId: {CorrelationId}", correlationId);
        // Call another service with the same "X-Correlation-Id" header when you set up the HttpClient
        //var httpContent = new StringContent("Hello world!");
        //httpContent.Headers.Add("X-Correlation-Id", correlationId);
        // Omitted for brevity

        return Enumerable.Range(1, 5).Select(index => new WeatherForecast
        {
            Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
            TemperatureC = Random.Shared.Next(-20, 55),
            Summary = Summaries[Random.Shared.Next(Summaries.Length)]
        })
        .ToArray();
    }


    [HttpGet("detail")]
    public ActionResult GetDetail()
    {
         // Get the "X-Correlation-Id" header from the request
        var correlationId = Request.Headers["X-CorrelationId"].FirstOrDefault();
        // Log the correlation ID
        _logger.LogInformation("Handling the request. CorrelationId: {CorrelationId}", correlationId);

      
        return Content(_studentService.StudentDetail() );
    }
}
