using System.Text;
using System.Text.Json;
using FavoriteItemV2.Data;
using FavoriteItemV2.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;

namespace FavoriteItemV2.Services;

public class ItemService(ILogger<ItemService> logger, ApplicationDbContext context, IDistributedCache distributedCache)
: IItemService
{
    public async Task<IEnumerable<Item>> GetItemsAsync()
    {
        // logger.LogInformation("Fetching all items from the database.");
        // return await context.Items.ToListAsync();

        var bytes = await distributedCache.GetAsync(CacheKeys.ItemCacheKey);
        if (bytes is { Length: > 0 })
        {
            logger.LogInformation($"Getting items from distributed cache");
            var cachedSerializedItems = Encoding.UTF8.GetString(bytes);
            var cachedItems = JsonSerializer.Deserialize<IEnumerable<Item>>(cachedSerializedItems);
            return cachedItems ?? new List<Item>();
        }
        // database query
        logger.LogInformation($"Fetching items from the database");
        var items = await context.Items.ToListAsync();
        await RefreshDistributedCacheAsync();
        return items;
    }

    public async Task<Item?> GetItemByIdAsync(int id)
    {
        // logger.LogInformation($"Fetching item with ID {id} from the database.");
        // return await context.Items.FindAsync(id);

        var cacheKey = $"{CacheKeys.ItemCacheKey}:{id}";
        var cacheEntryOptions = new DistributedCacheEntryOptions
        {
            SlidingExpiration = TimeSpan.FromMinutes(5),
            AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(10)
        };
        var item = await distributedCache.GetOrCreateAsync(cacheKey, async () =>
        {
            // querying database 
            logger.LogInformation($"Fetching item from the database with id {id}");
            var item = await context.Items.FindAsync(id);
            return item;
        }, cacheEntryOptions);
        return item;
    }

    public async Task<Item> CreateItemAsync(Item item)
    {
        context.Items.Add(item);
        await context.SaveChangesAsync();
        await RefreshDistributedCacheAsync();
        return item;
    }

    public async Task<bool> UpdateItemAsync(int id, Item item)
    {
        if (id != item.Id) return false;

        context.Entry(item).State = EntityState.Modified;

        try
        {
            await context.SaveChangesAsync();
            await RefreshDistributedCacheAsync();
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
        await RefreshDistributedCacheAsync();
        return true;
    }

    public async Task<IEnumerable<Item>> GetUserFavoritesAsync(string username)
    {
        //     // var existingUser = await context.Users.FindAsync(username);
        //     // FindAsync() looks only by primary key but username is not a primary key hence returns null
        //     // which breaks the subsequent logic
        //     var existingUser = await context.Users.FirstOrDefaultAsync(u => u.UserName == username);
        //     if (existingUser == null) return Enumerable.Empty<Item>();
        //     var userId = existingUser?.Id;

        //     var favoriteItems = await context.UserFavouriteItems
        //         .Where(f => f.UserId == userId)
        //         .Include(f => f.Item)
        //         .Select(f => f.Item)
        //         .ToListAsync();
        //     return favoriteItems;

        //Try to get the user favorites from the distributed cache
        // var cacheKey = $"{CacheKeys.UserFavoriteItemCacheKey}:{username}";
        // var bytes = await distributedCache.GetAsync(cacheKey);
        var bytes = await distributedCache.GetAsync(CacheKeys.UserFavoriteItemCacheKey(username));
        if (bytes is { Length: > 0 })
        {
            logger.LogInformation($"Getting user favorites from distributed cache for user {username}");
            var serializedFavoritesItems = Encoding.UTF8.GetString(bytes);
            var favoritesItems = JsonSerializer.Deserialize<IEnumerable<Item>>(serializedFavoritesItems);
            return favoritesItems ?? new List<Item>();
        }

        // database query
        logger.LogInformation($"Fetching user favorites from the database for user {username}");
        var existingUser = await context.Users.FirstOrDefaultAsync(u => u.UserName == username);
        if (existingUser == null) return Enumerable.Empty<Item>();
        var userId = existingUser?.Id;

        var favoriteItems = await context.UserFavouriteItems
            .Where(f => f.UserId == userId)
            .Include(f => f.Item)
            .Select(f => f.Item)
            .ToListAsync();

        // Store the result in the distributed cache
        var cacheEntryOptions = new DistributedCacheEntryOptions
        {
            SlidingExpiration = TimeSpan.FromMinutes(5),
            AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(10)
        };
        var serializedItems = JsonSerializer.Serialize(favoriteItems);
        var serializedItemsBytes = Encoding.UTF8.GetBytes(serializedItems);
        await distributedCache.SetAsync(CacheKeys.UserFavoriteItemCacheKey(username), serializedItemsBytes, cacheEntryOptions);
        return favoriteItems;

        // // implementing our own GetOrCreateAsync() extension method
        // var cacheKey = $"{CacheKeys.UserFavoriteItemCacheKey(username)}";
        // var cacheEntryOptions = new DistributedCacheEntryOptions
        // {
        //     SlidingExpiration = TimeSpan.FromMinutes(5),
        //     AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(10)
        // };
        // var favoritesItems = await distributedCache.GetOrCreateAsync(cacheKey, async () =>
        // {
        //     // querying database 
        //     logger.LogInformation($"Fetching user favorites from the database for user {username}");
        //     var existingUser = await context.Users.FirstOrDefaultAsync(u => u.UserName == username);
        //     if (existingUser == null) return Enumerable.Empty<Item>();
        //     var userId = existingUser?.Id;

        //     var favoriteItems = await context.UserFavouriteItems
        //         .Where(f => f.UserId == userId)
        //         .Include(f => f.Item)
        //         .Select(f => f.Item)
        //         .ToListAsync();
        //     return favoriteItems;
        // }, cacheEntryOptions);
        // return favoritesItems;
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
            await RefreshFavoriteDistributedCacheAsync(username);
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
        await RefreshFavoriteDistributedCacheAsync(username);
        return "Item removed from favorites";
    }

    private async Task RefreshDistributedCacheAsync()
    {
        // database query
        logger.LogInformation($"Fetching items from the database");
        var items = await context.Items.ToListAsync();

        // Store the result in the distributed cache
        var cacheEntryOptions = new DistributedCacheEntryOptions
        {
            SlidingExpiration = TimeSpan.FromMinutes(5),
            AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(10)
        };
        var serializedItems = JsonSerializer.Serialize(items);
        var serializedItemsBytes = Encoding.UTF8.GetBytes(serializedItems);
        await distributedCache.SetAsync(CacheKeys.ItemCacheKey, serializedItemsBytes, cacheEntryOptions);
    }

    private async Task RefreshFavoriteDistributedCacheAsync(string username)
    {
        // database query
        logger.LogInformation($"Fetching user favorites from the database for user {username}");
        var existingUser = await context.Users.FirstOrDefaultAsync(u => u.UserName == username);
        if (existingUser == null) return;
        var userId = existingUser?.Id;

        var favoriteItems = await context.UserFavouriteItems
            .Where(f => f.UserId == userId)
            .Include(f => f.Item)
            .Select(f => f.Item)
            .ToListAsync();

        // Store the result in the distributed cache
        var cacheEntryOptions = new DistributedCacheEntryOptions
        {
            SlidingExpiration = TimeSpan.FromMinutes(5),
            AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(10)
        };
        var serializedItems = JsonSerializer.Serialize(favoriteItems);
        var serializedItemsBytes = Encoding.UTF8.GetBytes(serializedItems);
        await distributedCache.SetAsync(CacheKeys.UserFavoriteItemCacheKey(username), serializedItemsBytes, cacheEntryOptions);
    }
}