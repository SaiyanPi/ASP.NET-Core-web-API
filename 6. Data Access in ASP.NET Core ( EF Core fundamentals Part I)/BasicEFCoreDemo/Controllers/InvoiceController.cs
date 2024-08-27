using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BasicEFCoreDemo.Data;
using BasicEFCoreDemo.Models;

namespace BasicEFCoreDemo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InvoiceController : ControllerBase
    {
        private readonly InvoiceDbContext _context;

        public InvoiceController(InvoiceDbContext context)
        {
            _context = context;
        }

        // GET: api/Invoice
        // [HttpGet]
        // public async Task<ActionResult<IEnumerable<Invoice>>> GetInvoices()
        // {
        //     return await _context.Invoices.ToListAsync();
        // }

        // 2. BASIC LINQ QUERIES/FILTERING THE DATA using Where() method:
        // [HttpGet]
        // public async Task<ActionResult<IEnumerable<Invoice>>> GetInvoices(InvoiceStatus? status)
        // {
        // // Omitted for brevity
        //     return await _context.Invoices.Where(x => status == null || x.Status == status).ToListAsync();
        // }

        // 3. BASIC LINQ QUERIES/SORTING AND PAGING:
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Invoice>>> GetInvoices(int
        page = 1, int pageSize = 10, InvoiceStatus? status = null)
        {
            // Omitted for brevity
            return await _context.Invoices.AsQueryable().Where(x => status == null || x.Status == status)
            .OrderByDescending(x => x.InvoiceDate)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();
        }


        // GET: api/Invoice/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Invoice>> GetInvoice(Guid id)
        {
            var invoice = await _context.Invoices.FindAsync(id);

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

            _context.Entry(invoice).State = EntityState.Modified;

            try
            {
                // update all properties:
                // await _context.SaveChangesAsync();

                // update only specified properties explicitly:
                var invoiceToUpdate = await _context.Invoices.FindAsync(id);
                if (invoiceToUpdate == null)
                {
                    return NotFound();
                }
                // invoiceToUpdate.InvoiceNumber = invoice.InvoiceNumber;
                // invoiceToUpdate.ContactName = invoice.ContactName;
                // invoiceToUpdate.Description = invoice.Description;
                // invoiceToUpdate.Amount = invoice.Amount;
                // invoiceToUpdate.InvoiceDate = invoice.InvoiceDate;
                // invoiceToUpdate.DueDate = invoice.DueDate;
                // invoiceToUpdate.Status = invoice.Status;
                // better way:
                _context.Entry(invoiceToUpdate).CurrentValues.SetValues(invoice);
                await _context.SaveChangesAsync();
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
        // [HttpPost]
        // public async Task<ActionResult<Invoice>> PostInvoice(Invoice invoice)
        // {
        //     _context.Invoices.Add(invoice);
        //     await _context.SaveChangesAsync();

        //     return CreatedAtAction("GetInvoice", new { id = invoice.Id }, invoice);
        // }

        [HttpPost]
        public async Task<ActionResult<Invoice>> PostInvoice(Invoice invoice)
        {
            if (_context.Invoices == null)
            {
                return Problem("Entity set 'InvoiceDbContext.Invoices' is null.");
            }
            // _context.Invoices.Add(invoice);
            _context.Entry(invoice).State = EntityState.Added; // this is equivalent to the preceding code
            await _context.SaveChangesAsync();
            return CreatedAtAction("GetInvoice", new { id = invoice.Id }, invoice);
        }

        // DELETE: api/Invoice/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteInvoice(Guid id)
        {
            //var invoice = await _context.Invoices.FindAsync(id);
            // better way:
            var invoice = new Invoice { Id = id };
            if (invoice == null)
            {
                return NotFound();
            }
            // _context.Entry(invoice).State = EntityState.Deleted; // this is equivalent to the folowing code
            _context.Invoices.Remove(invoice);
            await _context.SaveChangesAsync();
            // await _context.Invoices.Where(x => x.Id == id).ExecuteDeleteAsync();
            return NoContent();
        }

        private bool InvoiceExists(Guid id)
        {
            return _context.Invoices.Any(e => e.Id == id);
        }
    }
}
