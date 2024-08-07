using Microsoft.AspNetCore.Mvc;
using SecondApi2.Services;


namespace SecondApi2.Controllers;

[ApiController]
[Route("[controller]")]
public class KeyedServicesController : ControllerBase
{
    // accessing services with the key
    [HttpGet("sql")]
    public ActionResult GetSqlData([FromKeyedServices("sqlDatabaseService")] IDataService dataService) =>
        Content(dataService.GetData());

    [HttpGet("cosmos")]
    public ActionResult GetCosmosData([FromKeyedServices("cosmosDatabaseService")] IDataService dataService) =>
        Content(dataService.GetData());
}