using System.Security.Claims;
using FavoriteItemV2.Authentication;
using FavoriteItemV2.Data;
using FavoriteItemV2.Models;
using FavoriteItemV2.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.OutputCaching;

namespace FavoriteItemV2.Controllers;

[Authorize]
[ApiController]
[Route("[controller]")]
public class ItemController(IItemService itemService)
: ControllerBase
{
    //GET: item
    [HttpGet]
    [ResponseCache(Duration = 60)] // Response caching
    public async Task<IActionResult> GetItems()
    {
        var items = await itemService.GetItemsAsync();
        return Ok(items);
    }

    // GET: item/{id}
    [HttpGet("{id}")]
    // [OutputCache]
    public async Task<IActionResult> GetItem(int id)
    {
        var item = await itemService.GetItemByIdAsync(id);
        if (item == null) return NotFound("Item not found");
        return Ok(item);
    }

    // POST: item/add
    [HttpPost("add")]
    public async Task<IActionResult> CreateItem(Item item)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);

        var createdItem = await itemService.CreateItemAsync(item);
        return CreatedAtAction(nameof(GetItem), new { id = createdItem.Id }, createdItem);
    }

    // UPDATE: item/{id}
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateItem(int id, Item item)
    {
        if (id != item.Id) return BadRequest("Item ID mismatch");

        var updated = await itemService.UpdateItemAsync(id, item);
        if (!updated) return NotFound("Item not found");

        return NoContent();
    }

    // DELETE: item/{id}
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteItem(int id)
    {
        var item = await itemService.GetItemByIdAsync(id);
        if (item == null) return NotFound("Item not found");

        await itemService.DeleteItemAsync(id);
        return NoContent();
    }

    // GET: item/favorites
    [HttpGet("favorites")]
    public async Task<IActionResult> GetUserFavorites()
    {
        // ClaimTypes.Name already exists in the GenerateToken() method 
        var username = User.FindFirstValue(ClaimTypes.Name);

        var result = await itemService.GetUserFavoritesAsync(username);
        if(result == null || !result.Any())
        {
            return NotFound("No favorite items found for the user.");
        }
        return Ok(result);
    }

    // POST: item/{id}/favorite
    [HttpPost("{id}/favorite")]
    public async Task<IActionResult> AddToFavorites(int id)
    {
        // ClaimTypes.Name already exists in the GenerateToken() method 
        var username = User.FindFirstValue(ClaimTypes.Name);

        var item = await itemService.GetItemByIdAsync(id);
        if (item == null) return NotFound("Item not found");

        var result = await itemService.AddToFavoritesAsync(username, id);
        if(result == "User not found") return Unauthorized(result);
        if(result == "Item not found") return NotFound(result);
        if(result == "Item is already in favorites") return Conflict(result);
        return Ok(result);
    }

    // DELETE: item/{id}/favorite
    [HttpDelete("{id}/favorite")]
    public async Task<IActionResult> RemoveFromFavorites(int id)
    {
        var username = User.FindFirstValue(ClaimTypes.Name);

        var item = await itemService.GetItemByIdAsync(id);
        if (item == null) return NotFound("Item not found");
        
        var result = await itemService.RemoveFromFavoritesAsync(username, id);
        if (result == "User not found") return Unauthorized(result);
        if (result == "Item is not in favorites") return NotFound(result);

        return Ok(result);
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