using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ConcurrencyConflictDemo.Data;

namespace ConcurrencyConflictDemo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BooksController : ControllerBase
    {
        private readonly SampleDbContext _context;
        private readonly ILogger<BooksController> _logger;

        public BooksController(SampleDbContext context, ILogger<BooksController> logger)
        {
            _context = context;
            _logger = logger;

        }

        // GET: api/Books
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Book>>> GetBook()
        {
            return await _context.Book.ToListAsync();
        }

        // GET: api/Books/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Book>> GetBook(int id)
        {
            var book = await _context.Book.FindAsync(id);

            if (book == null)
            {
                return NotFound();
            }

            return book;
        }

        // PUT: api/Books/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutBook(int id, Book book)
        {
            if (id != book.Id)
            {
                return BadRequest();
            }

            _context.Entry(book).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!BookExists(id))
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

        // POST: api/Books
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Book>> PostBook(Book book)
        {
            _context.Book.Add(book);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetBook", new { id = book.Id }, book);
        }

        [HttpPost("{id}/sell/{quantity}")]
        public async Task<ActionResult<Book>> SellBook(int id, int quantity, int delay = 0)
        {
           if (_context.Products == null)
            {
                return Problem("Entity set 'SampleDbContext.Books' is null.");
            }
            var book = await _context.Book.FindAsync(id);
            if (book == null)
            {
                return NotFound();
            }
            if (book.Quantity < quantity)
            {
                return Problem("Not enough quantity.");
            }
            await Task.Delay(TimeSpan.FromSeconds(delay)); // Simulate delay for demonstration purposes
            book.Quantity -= quantity;
            // Manually assign a new value to the Version property.
            book.Version = Guid.NewGuid();
            // handling concurrency conflicts
            try
            {
                await _context.SaveChangesAsync();
            }
                catch (DbUpdateConcurrencyException)
            {
                // Do not forget to log the error
                _logger.LogInformation($"Concurrency conflict for Book {book.Id}.");
                return Conflict($"Concurrency conflict for Book {book.Id}.");
            }
            return book;
        }

        // DELETE: api/Books/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBook(int id)
        {
            var book = await _context.Book.FindAsync(id);
            if (book == null)
            {
                return NotFound();
            }

            _context.Book.Remove(book);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool BookExists(int id)
        {
            return _context.Book.Any(e => e.Id == id);
        }
    }
}
