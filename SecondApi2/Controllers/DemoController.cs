using Microsoft.AspNetCore.Mvc;
using SecondApi2.Services;

namespace SecondApi2.Controllers;

[ApiController]
[Route("api2/[controller]")]
public class DemoController : ControllerBase
{
    private readonly IDemoService _demoService;
    public DemoController(IDemoService demoService)
    {
        _demoService = demoService;
    }

    public ActionResult Get()
    {
        return Content(_demoService.SayWassup()); 
    }
}