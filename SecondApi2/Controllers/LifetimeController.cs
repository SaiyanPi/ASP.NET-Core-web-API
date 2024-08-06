using Microsoft.AspNetCore.Mvc;
using SecondApi2.Services;

namespace SecondApi2.Controllers;

[ApiController]
[Route("[controller]")]
public class LifetimeController : ControllerBase
{
    private readonly IScopedService _scopedService;
    private readonly ITransientService _transientService;
    private readonly ISingletonService _singletonService;

    public LifetimeController(IScopedService scopedService, ITransientService transientService,
    ISingletonService singletonService)
    {
        _scopedService = scopedService;
        _transientService = transientService;
        _singletonService = singletonService;
    }

    [HttpGet]
    public ActionResult Get()
    {
        var scopedServiceMessage = _scopedService.SayWassup();
        var transientServiceMessage = _transientService.SayWassup();
        var singletonServiceMessage = _singletonService.SayWassup();
        return Content(
            $"{scopedServiceMessage} {Environment.NewLine} {transientServiceMessage} {Environment.NewLine} {singletonServiceMessage}"
        );
    }
}