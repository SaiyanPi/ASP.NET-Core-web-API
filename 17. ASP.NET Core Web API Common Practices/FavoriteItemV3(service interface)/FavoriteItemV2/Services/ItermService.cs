using FavoriteItemV2.Data;
using FavoriteItemV2.Models;
using Microsoft.EntityFrameworkCore;

namespace FavoriteItemV2.Services;

public class ItemService(ILogger<ItemService> logger, ApplicationDbContext context) : IItemService
{
    public async Task<IEnumerable<Item>> GetItemsAsync()
    {
        logger.LogInformation("Fetching all items from the database.");
        return await context.Items.ToListAsync();
    }

    public async Task<Item?> GetItemByIdAsync(int id)
    {
        logger.LogInformation($"Fetching item with ID {id} from the database.");
        return await context.Items.FindAsync(id);
    }

    public async Task<Item> CreateItemAsync(Item item)
    {
        context.Items.Add(item);
        await context.SaveChangesAsync();
        return item;
    }

    public async Task<bool> UpdateItemAsync(int id, Item item)
    {
        if (id != item.Id) return false;

        context.Entry(item).State = EntityState.Modified;

        try
        {
            await context.SaveChangesAsync();
            return true;
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!context.Items.Any(e => e.Id == id)) return false;
            throw;
        }
    }

    public async Task<bool> DeleteItemAsync(int id)
    {
        var item = await context.Items.FindAsync(id);
        if (item == null) return false;

        context.Items.Remove(item);
        await context.SaveChangesAsync();
        return true;
    }

    public async Task<IEnumerable<Item>> GetUserFavoritesAsync(string username)
    {
        // var existingUser = await context.Users.FindAsync(username);
        // FindAsync() looks only by primary key but username is not a primary key hence returns null
        // which breaks the subsequent logic
        var existingUser = await context.Users.FirstOrDefaultAsync(u => u.UserName == username);
        if (existingUser == null) return Enumerable.Empty<Item>();
        var userId = existingUser?.Id;

        var favoriteItems = await context.UserFavouriteItems
            .Where(f => f.UserId == userId)
            .Include(f => f.Item)
            .Select(f => f.Item)
            .ToListAsync();

        return favoriteItems;
    }

    public async Task<string> AddToFavoritesAsync(string username, int itemId)
    {
        // var existingUser = await context.Users.FindAsync(username);
        var existingUser = await context.Users.FirstOrDefaultAsync(u => u.UserName == username);
        if (existingUser == null) return "User not found";
        var userId = existingUser?.Id;

        var existingItem = await context.Items.FindAsync(itemId);
        if (existingItem == null) return "Item not found";

        var exists = await context.UserFavouriteItems
            .AnyAsync(f => f.UserId == userId && f.ItemId == itemId);
        if (exists) return "Item is already in favorites";

        var favorite = new UserFavouriteItem
        {
            UserId = userId,
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
        }
        return "Item marked as favorite";
    }

    public async Task<string> RemoveFromFavoritesAsync(string username, int itemId)
    {
        // var existingUser = await context.Users.FindAsync(username);
        var existingUser = await context.Users.FirstOrDefaultAsync(u => u.UserName == username);
        if (existingUser == null) return "User not found";
        var userId = existingUser?.Id;

        var favoriteItem = await context.UserFavouriteItems
            .FirstOrDefaultAsync(f => f.UserId == userId && f.ItemId == itemId);

        if (favoriteItem == null) return "Item is not in favorites";

        context.UserFavouriteItems.Remove(favoriteItem);
        await context.SaveChangesAsync();
        return "Item removed from favorites";
    }
}