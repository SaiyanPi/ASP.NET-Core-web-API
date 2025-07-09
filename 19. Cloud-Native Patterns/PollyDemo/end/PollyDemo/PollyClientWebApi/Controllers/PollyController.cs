using Microsoft.AspNetCore.Mvc;
using Polly;
using Polly.Registry;

namespace PollyClientWebApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class PollyController(ILogger<PollyController> logger, IHttpClientFactory httpClientFactory,
    ResiliencePipelineProvider<string> resiliencePipelineProvider) : ControllerBase
{
    // [HttpGet("slow-response")]
    // public async Task<IActionResult> GetSlowResponse()
    // {
    //     var client = httpClientFactory.CreateClient("PollyServerWebApi");
    //     var response = await client.GetAsync("api/slow-response");
    //     var content = await response.Content.ReadAsStringAsync();
    //     return Ok(content);
    // }


    // timeout policy
    [HttpGet("slow-response")]
    public async Task<IActionResult> GetSlowResponse()
    {
        var client = httpClientFactory.CreateClient("PollyServerWebApi");

        // var pipeline = new ResiliencePipelineBuilder().AddTimeout(TimeSpan.FromSeconds(5)).Build();

        // global timeout policy
        var pipeline = resiliencePipelineProvider.GetPipeline("timeout-5s-pipeline");
        try
        {
            var response = await pipeline.ExecuteAsync(async cancellationToken =>
                await client.GetAsync("api/slow-response", cancellationToken));

            var content = await response.Content.ReadAsStringAsync();
            return Ok(content);
        }
        catch (Exception e)
        {
            logger.LogError(e.Message);
            return Problem(e.Message);
        }
    }
    
    [HttpGet("rate-limit")]
    public async Task<IActionResult> GetNormalResponseWithRateLimiting()
    {
        var client = httpClientFactory.CreateClient("PollyServerWebApi");
        try
        {
            var pipeline = resiliencePipelineProvider.GetPipeline("rate-limit-5-requests-in-3-seconds");
            var response = await pipeline.ExecuteAsync(async cancellationToken =>
                await client.GetAsync("api/normal-response", cancellationToken));
            var content = await response.Content.ReadAsStringAsync();
            return Ok(content);
        }
        catch (Exception e)
        {
            logger.LogError($"{e.GetType()} {e.Message}");
            return Problem(e.Message);
        }
    }
}