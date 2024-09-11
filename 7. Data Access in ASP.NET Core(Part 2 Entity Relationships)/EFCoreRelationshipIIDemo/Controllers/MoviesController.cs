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
    public class MoviesController(SampleDbContext context) : ControllerBase
    {

        // GET: api/Movies
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Movie>>> GetMovies()
        {
            return await context.Movies
            .ToListAsync();
        }

        // GET: api/Movies/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Movie>> GetMovie(Guid id)
        {
            var movie = await context.Movies.FindAsync(id);

            if (movie == null)
            {
                return NotFound();
            }

            return movie;
        }

        // GET actors of a movie
        [HttpGet("{id}/actors")]
        public async Task<IActionResult> GetActors(Guid id)
        {
            if (context.Movies == null)
            {
                return NotFound("Movies is null.");
            }
            var movie = await context.Movies
            .Include(x => x.Actors).SingleOrDefaultAsync();
           
            if (movie == null)
            {
                return NotFound($"Movie with id {id} not found.");
            }
            return Ok(movie.Actors);
        }

        // PUT: api/Movies/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutMovie(Guid id, Movie movie)
        {
            if (id != movie.Id)
            {
                return BadRequest();
            }

            context.Entry(movie).State = EntityState.Modified;

            try
            {
                await context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MovieExists(id))
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

        // POST: api/Movies
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Movie>> PostMovie(Movie movie)
        {
            context.Movies.Add(movie);
            await context.SaveChangesAsync();

            return CreatedAtAction("GetMovie", new { id = movie.Id }, movie);
        }

        // adding an actor to a movie
        [HttpPost("{id}/actors/{actorId}")]
        public async Task<IActionResult> AddActor(Guid id, Guid actorId)
        {
            if (context.Movies == null)
            {
                return NotFound("Movies is null.");
            }
            var movie = await context.Movies.Include(x => x.Actors).
            SingleOrDefaultAsync(x => x.Id == id);
            if (movie == null)
            {
                return NotFound($"Movie with id {id} not found."); 
            }
            var actor = await context.Actors.FindAsync(actorId);
            if (actor == null)
            {
                return NotFound($"Actor with id {actorId} not found.");
            }
            if (movie.Actors.Any(x => x.Id == actor.Id))
            {
                return Problem($"Actor with id {actorId} already exists for Movie {id}.");
            }
            movie.Actors.Add(actor);
            await context.SaveChangesAsync();
            return CreatedAtAction("GetMovie", new { id = movie.Id }, movie);
        }

        // DELETE: api/Movies/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMovie(Guid id)
        {
            var movie = await context.Movies.FindAsync(id);
            if (movie == null)
            {
                return NotFound();
            }

            context.Movies.Remove(movie);
            await context.SaveChangesAsync();

            return NoContent();
        }

        // delete an actor from a movie
        [HttpDelete("{id}/actors/{actorId}")]
        public async Task<IActionResult> DeleteActor(Guid id, Guid actorId)
        {
            if (context.Movies == null)
            {
                return NotFound("Movies is null.");
            }
            var movie = await context.Movies.Include(x => x.Actors).
            SingleOrDefaultAsync();
            if (movie == null)
            {
            return NotFound($"Movie with id {id} not found.");
            }
            var actor = await context.Actors.FindAsync(actorId);
            if (actor == null)
            {
                return NotFound($"Actor with id {actorId} not found.");
            }
            movie.Actors.Remove(actor);
            await context.SaveChangesAsync();
            return NoContent();
        }

        private bool MovieExists(Guid id)
        {
            return context.Movies.Any(e => e.Id == id);
        }
    }
}
