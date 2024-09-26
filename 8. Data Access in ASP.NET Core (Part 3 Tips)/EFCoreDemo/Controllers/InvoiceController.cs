using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using EFCoreDemo.Data;
using EFCoreDemo.Models;
using EfCoreDemo;
using Microsoft.Data.SqlClient;

namespace EFCoreDemo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InvoiceController(SampleDbContext context, ILogger<SampleDbContext> logger) : ControllerBase
    {

        // GET: api/Invoice
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Invoice>>> GetInvoices(int
        page = 1, int pageSize = 10, InvoiceStatus? status = null)
        {
              if (context.Invoices == null)
            {
                return NotFound();
            }
            
            // Use IQueryable
            logger.LogInformation($"Creating the IQueryable...");
            var list1 = context.Invoices.Where(x => status == null || x.Status == status);
            logger.LogInformation($"IQueryable created");
            logger.LogInformation($"Query the result using IQueryable...");
            var query1 = list1.OrderByDescending(x => x.InvoiceDate)
            .Skip((page - 1) * pageSize)
            .Take(pageSize);
            logger.LogInformation($"Execute the query using IQueryable");
            var result1 = await query1.ToListAsync();
            logger.LogInformation($"Result created using IQueryable");

            // Use IEnumerable
            logger.LogInformation($"Creating the IEnumerable...");
            var list2 = context.Invoices.Where(x => status == null || x.Status == status)
                .AsEnumerable();
            logger.LogInformation($"IEnumerable created");
            logger.LogInformation($"Query the result using IEnumerable...");
            var query2 = list2.OrderByDescending(x => x.InvoiceDate)
            .Skip((page - 1) * pageSize)
            .Take(pageSize);
            logger.LogInformation($"Execute the query using IEnumerable");
            var result2 = query2.ToList();
            logger.LogInformation($"Result created using IEnumerable");


            return result1;
        }

        // client evaluation vs server evaluation:
        [HttpGet]
        [Route("search")]
        public async Task<ActionResult<IEnumerable<Invoice>>> SearchInvoices(string search)
        {
            if (context.Invoices == null)
            {
                return NotFound();
            }

            var list = await context.Invoices
                // The below code will throw an exception if the CalculatedTax method is not static
                //.Where(x => (x.ContactName.Contains(search) || x.InvoiceNumber.Contains(search)) && CalculateTax(x.Amount) > 10)
                .Where(x => (x.ContactName.Contains(search) || x.InvoiceNumber.Contains(search)))
                .Select(x => new Invoice
                {
                    Id = x.Id,
                    InvoiceNumber = x.InvoiceNumber,
                    ContactName = x.ContactName,
                    // The below conversion will be executed on the client side
                    Description = $"Tax: ${CalculateTax(x.Amount)}. {x.Description}",
                    Amount = x.Amount,
                    InvoiceDate = x.InvoiceDate,
                    DueDate = x.DueDate,
                    Status = x.Status
                })
                .ToListAsync();
            return list;
        }
        private static decimal CalculateTax(decimal amount)
        {
            return amount * 0.15m;
        }

        // FromSql() method for raw SQL query:
        [HttpGet]
        [Route("status")]
        public async Task<ActionResult<IEnumerable<Invoice>>> GetInvoices(string status)
        {
            if (context.Invoices == null)
            {
                return NotFound();
            }
            var list = await context.Invoices
                .FromSql($"SELECT * FROM Invoices WHERE Status = {status}")
                .ToListAsync();
            return list;
        }

        // FromSqlRaw() method for raw dynamic SQL queries:
        [HttpGet]
        [Route("free-search")]
        public async Task<ActionResult<IEnumerable<Invoice>>> GetInvoices(string propertyName, string propertyValue)
        {
            if (context.Invoices == null)
            {
                return NotFound();
            }

            var value = new SqlParameter("value", propertyValue);

            var list = await context.Invoices
                .FromSqlRaw($"SELECT * FROM Invoices WHERE {propertyName} = @value", value)
                .ToListAsync();
            return list;
        }
        
        //SqlQuery():
        [HttpGet]
        [Route("ids")]
        public ActionResult<IEnumerable<Guid>> GetInvoicesIds(string status)
        {
            if (context.Invoices == null)
            {
                return NotFound();
            }

            var result = context.Database
                .SqlQuery<Guid>($"SELECT Id FROM Invoices WHERE Status = {status}")
                .ToList();
            return result;
        }

        // ExecuteSql()
        [HttpDelete]
        [Route("status")]
        public async Task<ActionResult> DeleteInvoices(string status)
        {
            var result = await context.Database
                .ExecuteSqlAsync($"DELETE FROM Invoices WHERE Status = {status}");
            return Ok();
        }
        
        // bulk operations/ExecuteUpdate()
        [HttpPut]
        [Route("status/cancelled")]
        public async Task<ActionResult> UpdateInvoicesStatusAsCancelled(DateTime date)
        {
            var result = await context.Invoices
                // get all invoices with InvoiceStatus Overdue and created before given data in the query
                .Where(i => i.InvoiceDate < date && i.Status == InvoiceStatus.Overdue)
                // ExecuteUpdate single property(status)
                .ExecuteUpdateAsync(s => s.SetProperty(x => x.Status, InvoiceStatus.Cancelled));
            return Ok();
        }

        [HttpPut]
        [Route("status/paid")]
        public async Task<ActionResult> UpdateInvoicesStatusAndAmount(DateTime date)
        {
            var result = await context.Invoices
                // get all invoices with InvoiceStatus Overdue and created before given data in the query
                .Where(i => i.InvoiceDate < date && i.Status == InvoiceStatus.Draft)
                // ExecuteUpdate multiplt property(status and amount)
                .ExecuteUpdateAsync(s =>
                    s.SetProperty(x => x.Status, InvoiceStatus.Cancelled)
                    .SetProperty(x => x.Amount, 200));
            return Ok();
        }

        // bulk operations/ExecuteDelete()
        [HttpDelete]
        [Route("status/await")]
        public async Task<ActionResult> DeleteInvoicesByStatus(string status)
        {
            await context.Invoices.Where(x => x.Status == InvoiceStatus.AwaitPayment)
                .ExecuteDeleteAsync();
            return Ok();
        }
        
        // GET: api/Invoice/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Invoice>> GetInvoice(Guid id)
        {
            if(context.Invoices == null)
            {
                return NotFound();
            }
            logger.LogInformation($"Invoice {id} is loading from the database.");
            var invoice = await context.Invoices.FindAsync(id);
            // Using No-Tracking query to improve performance.
            // var  invoice = await context.Invoices.AsNoTracking()
            //     .FirstOrDefaultAsync(x => x.Id == id);
            logger.LogInformation($"Invoice {invoice?.Id} is loaded from the database.");
            logger.LogInformation($"Invoice {id} is loading from the context.");
            invoice = await context.Invoices.FindAsync(id);
            logger.LogInformation($"Invoice {invoice?.Id} is loaded from the context.");
            if (invoice == null)
            {
                return NotFound();
            }

            return invoice;
        }

        // PUT: api/Invoice/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutInvoice(Guid id, Invoice invoice)
        {
            if (id != invoice.Id)
            {
                return BadRequest();
            }

            context.Entry(invoice).State = EntityState.Modified;

            try
            {
                await context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!InvoiceExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Invoice
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Invoice>> PostInvoice(Invoice invoice)
        {
            context.Invoices.Add(invoice);
            await context.SaveChangesAsync();

            return CreatedAtAction("GetInvoice", new { id = invoice.Id }, invoice);
        }

        // DELETE: api/Invoice/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteInvoice(Guid id)
        {
            // var invoice = await context.Invoices.FindAsync(id);
            // if (invoice == null)
            // {
            //     return NotFound();
            // }
            // context.Invoices.Remove(invoice);
            // await context.SaveChangesAsync();

            // The following way does not need to find the entity first.
            var invoice = new Invoice { Id = id };
            context.Invoices.Remove(invoice);
            await context.SaveChangesAsync();

            // using ExecuteSql
            // await context.Database.ExecuteSqlAsync($"DELETE FROM Invoices WHERE Id = {id}");

            // using bulk operations/ExecuteDelete() available from EF Core 7.0
            // await context.Invoices.Where(x => x.Id == id ).ExecuteDeletAsync();

            return NoContent();
        }

        private bool InvoiceExists(Guid id)
        {
            return context.Invoices.Any(e => e.Id == id);
        }
    }
}
