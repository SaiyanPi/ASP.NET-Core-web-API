using Microsoft.AspNetCore.Http.Timeouts;
using Microsoft.AspNetCore.Mvc;


namespace MiddlewareDemo.Controllers;

[ApiController]
[Route("[controller]")]
public class RequestTimeoutController(ILogger<RequestTimeoutController> logger) : ControllerBase
{
    private readonly Random _random = new();
    
    [HttpGet("request-timeout")]
    // [RequestTimeout(5000)] // without policy
    [RequestTimeout("ShortTimeoutPolicy")] // with policy
    public async Task<ActionResult> RequestTimeoutDemo()
    {
        var delay = _random.Next(1, 10);
        logger.LogInformation($"Delaying for {delay} seconds");
        try
        {
            await Task.Delay(TimeSpan.FromSeconds(delay), Request.HttpContext.RequestAborted);
        }
        catch
        {
            logger.LogWarning("The request timed out");
            return StatusCode(StatusCodes.Status503ServiceUnavailable, "The request timed out");
        }
        return Ok($"Hello! The task is complete in {delay} seconds");
    }

    [HttpGet("request-timeout-short")]
    [RequestTimeout("ShortTimeoutPolicy")]
    public async Task<ActionResult> RequestTimeoutShortDemo()
    {
        var delay = _random.Next(1, 10);
        logger.LogInformation($"Delaying for {delay} seconds");
        try
        {
            await Task.Delay(TimeSpan.FromSeconds(delay), Request.HttpContext.RequestAborted);
        }
        catch
        {
            logger.LogWarning("The request timed out");
            return StatusCode(StatusCodes.Status503ServiceUnavailable, "The request timed out");
        }
        return Ok($"Hello! The task is complete in {delay} seconds");
    }

    [HttpGet("request-timeout-long")]
    [RequestTimeout("LongTimeoutPolicy")]
    public async Task<ActionResult> RequestTimeoutLongDemo()
    {
        // Get the "X-Correlation-Id" header from the request
        var correlationId = Request.Headers["X-Correlation-Id"].FirstOrDefault();
        // Log the correlation ID
        logger.LogInformation("Handling the request. CorrelationId: {CorrelationId}", correlationId);
        
        
        var delay = _random.Next(1, 10);
        logger.LogInformation($"Delaying for {delay} seconds");
        try
        {
            await Task.Delay(TimeSpan.FromSeconds(delay), Request.HttpContext.RequestAborted);
        }
        catch
        {
            logger.LogWarning("The request timed out");
            return StatusCode(StatusCodes.Status503ServiceUnavailable, "The request timed out");
        }
        return Ok($"Hello! The task is complete in {delay} seconds");
    }
}