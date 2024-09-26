using Microsoft.AspNetCore.Mvc;

namespace LoggingDemo;

[ApiController]
[Route("[controller]")]
public class LoggingController(ILogger<LoggingController> logger) : ControllerBase
{
    [HttpGet]
    [Route("structured-logging")]
    public ActionResult StructuredLoggingSample()
    {
        logger.LogInformation("This is a logging message with args: Today is {Week}. It is {Time}.", 
        DateTime.Now.DayOfWeek, DateTime.Now.ToLongTimeString());
        logger.LogInformation($"This is a logging message with string concatenation: Today is {DateTime.Now.DayOfWeek}. It is {DateTime.Now.ToLongTimeString()}.");
        return Ok("This is to test the difference between structured logging and string concatenation.");
    }
}z