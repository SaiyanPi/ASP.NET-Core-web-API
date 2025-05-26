using FavoriteItemV2.Services;
using Microsoft.Extensions.Caching.Memory;

namespace FavoriteItemV2;
public class ItemsCacheBackgroundService(IServiceProvider serviceProvider,
ILogger<ItemsCacheBackgroundService> logger,
IMemoryCache cache) 
: BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        // Remove the cache every 1 hour
        while (!stoppingToken.IsCancellationRequested)
        {
            logger.LogInformation("Updating the cache in background service");
            using var scope = serviceProvider.CreateScope();
            var itemService = scope.ServiceProvider.GetRequiredService<IItemService>();
            var items = await itemService.GetItemsAsync();
            cache.Remove(CacheKeys.ItemCacheKey);
            cache.Set(CacheKeys.ItemCacheKey, items, TimeSpan.FromHours(1));
            await Task.Delay(TimeSpan.FromHours(1), stoppingToken);
        }
    }
}