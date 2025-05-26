using FavoriteItemV2.Data;
using FavoriteItemV2.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;

namespace FavoriteItemV2.Services;

public class ItemService(ILogger<ItemService> logger, ApplicationDbContext context, IMemoryCache cache)
: IItemService
{
    public async Task<IEnumerable<Item>> GetItemsAsync()
    {
        if (cache.TryGetValue(CacheKeys.ItemCacheKey, out IEnumerable<Item> cachedItems))
        {
            logger.LogInformation("Returning items from cache.");
            return cachedItems ?? new List<Item>();
        }

        logger.LogInformation("Fetching all items from the database.");
        var items = await context.Items.ToListAsync();
        await RefreshCacheAsync();
        return items;
    }

    public async Task<Item?> GetItemByIdAsync(int id)
    {
        // this code is commented because if the id doesn't exist, the cache will not set and database
        // will query everytime.
        // if (cache.TryGetValue($"{CacheKeys.ItemCacheKey}:{id}", out Item? item))
        // {
        //     logger.LogInformation($"Returning item with id {id} from cache");
        //     return item;
        // }
        // logger.LogInformation($"Fetching item with ID {id} from the database.");
        // item = await context.Items.FindAsync(id);
        // if (item is not null)
        // {
        //     cache.Set($"{CacheKeys.ItemCacheKey}:{id}", item);
        // }
        // return item;

        // Here cache is set so the database will not query everytime
        var item = await cache.GetOrCreateAsync($"{CacheKeys.ItemCacheKey}:{id}", async entry =>
        {
            logger.LogInformation($"Fetching item with ID {id} from the database.");
            var result = await context.Items.FindAsync(id);
            return result;
        });
        return item;
    }

    public async Task<Item> CreateItemAsync(Item item)
    {
        context.Items.Add(item);
        await context.SaveChangesAsync();
        await RefreshCacheAsync();
        return item;
    }

    public async Task<bool> UpdateItemAsync(int id, Item item)
    {
        if (id != item.Id) return false;

        context.Entry(item).State = EntityState.Modified;

        try
        {
            await context.SaveChangesAsync();
            await RefreshCacheAsync();
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
        await RefreshCacheAsync();
        return true;
    }

    public async Task<IEnumerable<Item>> GetUserFavoritesAsync(string username)
    {
        // // var existingUser = await context.Users.FindAsync(username);
        // // FindAsync() looks only by primary key but username is not a primary key hence returns null
        // // which breaks the subsequent logic
        // var existingUser = await context.Users.FirstOrDefaultAsync(u => u.UserName == username);
        // if (existingUser == null) return Enumerable.Empty<Item>();
        // var userId = existingUser?.Id;

        // var favoriteItems = await context.UserFavouriteItems
        //     .Where(f => f.UserId == userId)
        //     .Include(f => f.Item)
        //     .Select(f => f.Item)
        //     .ToListAsync();

        // return favoriteItems;

        // Try to get cached value
        if (cache.TryGetValue(CacheKeys.UserFavoriteItemCacheKey(username), out IEnumerable<Item> cachedItems))
        {
            logger.LogInformation($"Returning favorite items for user {username} from cache.");
            return cachedItems ?? new List<Item>();
        }

        // cache miss - fetch from database
        logger.LogInformation($"Fetching favorite items for user {username} from the database.");
        var existingUser = await context.Users.FirstOrDefaultAsync(u => u.UserName == username);
        if (existingUser == null) return Enumerable.Empty<Item>();
        var userId = existingUser?.Id;

        var favoriteItems = await context.UserFavouriteItems
            .Where(f => f.UserId == userId)
            .Include(f => f.Item)
            .Select(f => f.Item)
            .ToListAsync();
        //store in cache for 5 minutes
        // cache.Set(CacheKeys.UserFavoriteItemCacheKey(username), favoriteItems, TimeSpan.FromMinutes(5));
        await RefreshFavoriteCacheAsync(username);
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
            await RefreshFavoriteCacheAsync(username);
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
        await RefreshFavoriteCacheAsync(username);
        return "Item removed from favorites";
    }

    private async Task RefreshCacheAsync()
    {
        logger.LogInformation("Fetching all items from the database");
        var items = await context.Items.ToListAsync();
        cache.Remove(CacheKeys.ItemCacheKey);
        var cacheEntryOptions = new MemoryCacheEntryOptions
        {
            AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(5),
            SlidingExpiration = TimeSpan.FromMinutes(2)
        };

        cache.Set(CacheKeys.ItemCacheKey, items, cacheEntryOptions);
    }
    
    private async Task RefreshFavoriteCacheAsync(string username)
    {
        logger.LogInformation($"Fetching favorite items for user {username} from the database");
        var existingUser = await context.Users.FirstOrDefaultAsync(u => u.UserName == username);
        if (existingUser == null) return;
        var userId = existingUser?.Id;

        var favoriteItems = await context.UserFavouriteItems
            .Where(f => f.UserId == userId)
            .Include(f => f.Item)
            .Select(f => f.Item)
            .ToListAsync();

        cache.Remove(CacheKeys.UserFavoriteItemCacheKey(username));

        var cacheEntryOptions = new MemoryCacheEntryOptions
        {
            SlidingExpiration = TimeSpan.FromMinutes(10),
            AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(30)
        };
        cache.Set(CacheKeys.UserFavoriteItemCacheKey(username), favoriteItems, cacheEntryOptions);
    }
}