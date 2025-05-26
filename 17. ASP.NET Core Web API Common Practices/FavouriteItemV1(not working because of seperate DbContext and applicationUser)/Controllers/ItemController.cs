using System.Security.Claims;
using FavouriteItemApi.Authentication;
using FavouriteItemApi.Data;
using FavouriteItemApi.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FavouriteItemApi.Controllers;

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

    [HttpGet("{id}")]
    public async Task<IActionResult> GetItem(int id)
    {
        var item = await context.Items.FindAsync(id);
        if (item == null) return NotFound("Item not found");
        return Ok(item);
    }

    // GET: item/favorites
    [HttpGet("favorites")]
    public async Task<IActionResult> GetUserFavorites()
    {
        var user = await userManager.GetUserAsync(User);
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

    // for clear understanding (trying to get logged in user's details)
    [HttpGet("user")]
    public async Task<IActionResult> GetUser()
    {
        var username = User.FindFirstValue(ClaimTypes.Name);
        if (username == null) return Unauthorized("User not found");
        var user = await userManager.FindByNameAsync(username);
        var id = user.Id;
        var email = user.Email;
        return Ok(new { id, email });
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
        var existingUserName = user.UserName;
        var existingUserEmail = user.Email;

        var item = await context.Items.FindAsync(id);
        if (item == null) return NotFound("Item not found");
        var itemId = item.Id;

        var exists = await context.UserFavouriteItems
            .AnyAsync(f => f.UserId == existingUserId && f.ItemId == itemId);

        if (exists) return BadRequest("Item already marked as favorite");

        // inserting user detail to ApplicationUser table first because ApplicationUser doesn't have a 
        // user's data since it only inhertis from Identity table AspNetUsers.
        // if we do not save the user to the ApplicationUser table first, we will get a ForeignKey constraint
        // conflict error. since we don't already have a reference data(Id) in the referenced entity(ApplicatiionUser)
        // but it is not the right way because if we make the endpoint request again, it will try to
        //  insert the same user data in the  ApplicationUser table hence giving "violation of PRIMARY KEY
        // constraint 'PK_ApplicationUser'. Cannot insert duplicate key in object 'dbo." error
        // The reason we're getting this error is because in the table ApplicationUser, the Id is the primary key\
        // and we can't have a duplicate Id data.
        // So, the solution might be adding another non-primary property/column to the ApplicationUser entity/table to the
        // to store the Id so that we are allowed to have a duplicate Id in the table under newly added property.
        // But again this is not the right way so what if we save the user data in the ApplicationUser table
        // simultaneously while registering user in the Identity table AspNetUsers?
        // well... after a bit research, ChatGpt recommended me to collapse the inheritance and just use one 
        // class as:
        // public class ApplicationUser : IdentityUser
        // {
        //     public ICollection<Item> Items { get; set; } = new();
        //     public ICollection<UserFavouriteItem> FavouriteItem { get; set; } = new();
        // }
        // elimitaing the ApplicationUser entity because obviously they both stores the same data
        // (See FavoriteItemV2)

        var applicationUser = new ApplicationUser
        {
            Id = existingUserId,
            UserName = existingUserName,
            Email = existingUserEmail
        };
        context.ApplicationUser.Add(applicationUser);
        await context.SaveChangesAsync();
    
        var favorite = new UserFavouriteItem
        {
            UserId = existingUserId,
            ItemId = itemId
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
}