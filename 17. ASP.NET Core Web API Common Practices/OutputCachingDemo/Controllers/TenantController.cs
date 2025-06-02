using System.Text;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OutputCaching;
using Microsoft.EntityFrameworkCore;
using OutputCaching.Data;
using OutputCaching.Models;

namespace OutputCaching.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TenantController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public TenantController(ApplicationDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    // [OutputCache(PolicyName = "Expire3600")] >> NOT WORKING
    public async Task<ActionResult<IEnumerable<Tenant>>> GetTenants()
    {
        // Console.WriteLine(">>> HIT /api/tenants");
        if (_context.Tenants == null)
        {
            return NotFound();
        }
        var tenants = await _context.Tenants.AsNoTracking().ToListAsync();
        return Ok(tenants);
        // var json = System.Text.Json.JsonSerializer.Serialize(tenants);
        // var bytes = Encoding.UTF8.GetBytes(json);
        // Response.Headers["Content-Length"] = bytes.Length.ToString();
        // return Ok(tenants);
       
    }

    [HttpGet("{id:guid}")]
    [OutputCache(PolicyName = "Expire3600")]
    public async Task<ActionResult<Tenant>> GetTenant(Guid id)
    {
        var tenant = await _context.Tenants.AsNoTracking().FirstOrDefaultAsync(t => t.Id == id);
        if (tenant == null)
        {
            return NotFound();
        }
        return Ok(tenant);
    }
}