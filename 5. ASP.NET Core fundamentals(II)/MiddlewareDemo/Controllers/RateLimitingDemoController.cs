using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;

namespace MiddlewareDemo.Controllers;

[ApiController]
[Route("[controller]")]
public class RateLimitingDemoController : ControllerBase
{
    [EnableRateLimiting(policyName: "fixed")]
    [HttpGet("rate-limiting")]
    public ActionResult RateLimitingDemo()
    {
        return Ok($"Hello {DateTime.Now.Ticks.ToString()}");
    }
}