using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using EFCoreRelationshipIIDemo.Data;
using EFCoreRelationshipIIDemo.Models;

namespace EFCoreRelationshipIIDemo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookController : ControllerBase
    {
        private readonly SampleDbContext _context;

        public BookController(SampleDbContext context)
        {
            _context = context;
        }

        // GET: api/Book
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Book>>> GetBooks()
        {
            return await _context.Books.ToListAsync();
        }

        // GET: api/Book/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Book>> GetBook(Guid id)
        {
            var book = await _context.Books.FindAsync(id);

            if (book == null)
            {
                return NotFound();
            }

            return book;
        }

        // get genre of the book
        [HttpGet("{id}/genres")]
        public async Task<IActionResult> GetGenres(Guid id)
        {
            if(_context.Books == null)
            {
                return NotFound("Books is null.");
            }
            var book = await _context.Books
            .Include(b => b.Genres)
            .SingleOrDefaultAsync();

            if(book == null)
            {
                return NotFound("Book with id {id} not found.");
            }
            return Ok(book.Genres);
        }

        // PUT: api/Book/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutBook(Guid id, Book book)
        {
            if (id != book.BookId)
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

        // adding a genre to a book
        [HttpPost("{id}/genres/{genreId}")]
        public async Task<IActionResult> AddGenre(Guid id, Guid genreId)
        {
            if (_context.Books == null)
            {
                return NotFound("Books is null.");
            }
            var book = await _context.Books.Include(x => x.Genres).
            SingleOrDefaultAsync(x => x.BookId == id);
            if (book == null)
            {
                return NotFound($"Book with id {id} not found."); 
            }
            var genre = await _context.Genres.FindAsync(genreId);
            if (genre == null)
            {
                return NotFound($"Genre with id {genreId} not found.");
            }
            if (book.Genres.Any(x => x.GenId == genre.GenId))
            {
                return Problem($"Genre with id {genreId} already exists for Book {id}.");
            }
            book.Genres.Add(genre);
            await _context.SaveChangesAsync();
            return CreatedAtAction("GetBooks", new { id = book.BookId }, book);
        }

        // POST: api/Book
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Book>> PostBook(Book book)
        {
            _context.Books.Add(book);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetBook", new { id = book.BookId }, book);
        }

        // DELETE: api/Book/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBook(Guid id)
        {
            var book = await _context.Books.FindAsync(id);
            if (book == null)
            {
                return NotFound();
            }

            _context.Books.Remove(book);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        //deleting a genre from a book
        [HttpDelete("{id}/genres/{genreId}")]
         public async Task<IActionResult> DeleteGenre(Guid id, Guid genreId)
        {
            if (_context.Books == null)
            {
                return NotFound("Books is null.");
            }
            var book = await _context.Books.Include(x => x.Genres).
            SingleOrDefaultAsync();
            if (book == null)
            {
            return NotFound($"Book with id {id} not found.");
            }
            var genre = await _context.Genres.FindAsync(genreId);
            if (genre == null)
            {
                return NotFound($"Genre with id {genreId} not found.");
            }
            book.Genres.Remove(genre);
            await _context.SaveChangesAsync();
            return NoContent();
        }

        private bool BookExists(Guid id)
        {
            return _context.Books.Any(e => e.BookId == id);
        }
    }
}
