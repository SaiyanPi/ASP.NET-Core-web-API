using System.Security.Claims;
using FavoriteItemV2.Authentication;
using FavoriteItemV2.Data;
using FavoriteItemV2.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FavoriteItemV2.Controllers;

[Authorize]
[ApiController]
[Route("[controller]")]
public class ItemController(ApplicationDbContext context, UserManager<AppUser> userManager)
: ControllerBase
{
    //GET: item
    [HttpGet]
    public async Task<IActionResult> GetItems()
    {
        var items = await context.Items.ToListAsync();
        return Ok(items);
    }

    // GET: item/{id}
    [HttpGet("{id}")]
    public async Task<IActionResult> GetItem(int id)
    {
        var item = await context.Items.FindAsync(id);
        if (item == null) return NotFound("Item not found");
        return Ok(item);
    }

    // POST: item/add
    [HttpPost("add")]
    public async Task<IActionResult> CreateItem(Item item)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);

        context.Items.Add(item);
        await context.SaveChangesAsync();
        return CreatedAtAction(nameof(GetItem), new { id = item.Id }, item);
    }

    // UPDATE: item/{id}
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateItem(int id, Item item)
    {
        if (id != item.Id) return BadRequest("Item ID mismatch");

        context.Entry(item).State = EntityState.Modified;

        try
        {
            await context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!context.Items.Any(e => e.Id == id)) return NotFound("Item not found");
            throw;
        }

        return NoContent();
    }

    // DELETE: item/{id}
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteItem(int id)
    {
        var item = await context.Items.FindAsync(id);
        if (item == null) return NotFound("Item not found");

        context.Items.Remove(item);
        await context.SaveChangesAsync();
        return NoContent();
    }
    
    // GET: item/favorites
    [HttpGet("favorites")]
    public async Task<IActionResult> GetUserFavorites()
    {
        var username = User.FindFirstValue(ClaimTypes.Name);
        var user = await userManager.FindByNameAsync(username);
        if (user == null) return Unauthorized("User not found");

        var favoriteItems = await context.UserFavouriteItems
            .Where(f => f.UserId == user.Id)
            .Include(f => f.Item)
            .Select(f => new
            {
                f.Item.Id,
                f.Item.Name
            })
            .ToListAsync();

        return Ok(favoriteItems);
    }

    // POST: item/{id}/favorite
    [HttpPost("{id}/favorite")]
    public async Task<IActionResult> AddToFavorites(int id)
    {
        // ClaimTypes.Name already exists in the GenerateToken() method 
        var username = User.FindFirstValue(ClaimTypes.Name);
        var user = await userManager.FindByNameAsync(username);
        if (user == null) return Unauthorized("User not found");
        var existingUserId = user.Id;

        var item = await context.Items.FindAsync(id);
        if (item == null) return NotFound("Item not found");

        var exists = await context.UserFavouriteItems
            .AnyAsync(f => f.UserId == existingUserId && f.ItemId == id);

        if (exists) return BadRequest("Item already marked as favorite");

        var favorite = new UserFavouriteItem
        {
            UserId = existingUserId,
            ItemId = id
        };

        context.UserFavouriteItems.Add(favorite);
        try
        {
            await context.SaveChangesAsync();
        }
        catch (DbUpdateException ex)
        {
            var innerMessage = ex.InnerException?.Message;
            Console.WriteLine(innerMessage);
            return BadRequest(innerMessage);
        }
        return Ok("Item marked as favorite");
    }

    // DELETE: item/{id}/favorite
    [HttpDelete("{id}/favorite")]
    public async Task<IActionResult> RemoveFromFavorites(int id)
    {
        var username = User.FindFirstValue(ClaimTypes.Name);
        var user = await userManager.FindByNameAsync(username);
        if (user == null) return Unauthorized("User not found");

        var favorite = await context.UserFavouriteItems
            .FirstOrDefaultAsync(f => f.UserId == user.Id && f.ItemId == id);
        if (favorite == null) return NotFound("Favorite not found");

        context.UserFavouriteItems.Remove(favorite);
        await context.SaveChangesAsync();
        return Ok("Item removed from favorites");
    }

    // // for clear understanding (trying to get logged in user's details)
    // [HttpGet("user")]
    // public async Task<IActionResult> GetUser()
    // {
    //     var username = User.FindFirstValue(ClaimTypes.Name);
    //     if (username == null) return Unauthorized("User not found");
    //     var user = await userManager.FindByNameAsync(username);
    //     var id = user.Id;
    //     var email = user.Email;
    //     return Ok(new { id, email });
    // }
}