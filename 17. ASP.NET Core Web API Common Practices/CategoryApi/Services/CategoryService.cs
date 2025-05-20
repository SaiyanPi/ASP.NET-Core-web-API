using CategoryApi.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;

namespace CategoryApi.Services;

public class CategoryService : ICategoryService
{
    private readonly CategoryDbContext _dbContext;
    private readonly ILogger<CategoryService> _logger;
    private readonly IMemoryCache _cache;

    public CategoryService(CategoryDbContext dbContext, ILogger<CategoryService> logger, IMemoryCache cache)
    {
        _dbContext = dbContext;
        _logger = logger;
        _cache = cache;
    }

    private async Task RefreshCacheAsync()
    {
        //query the database first
        _logger.LogInformation("Fetching all categories from the database.");
        var categories = await _dbContext.Categories.ToListAsync();
        // Then Refresh the cache
        _cache.Remove(CacheKeys.Categories);

        // var cacheEntryOptions = new MemoryCacheEntryOptions()
        //     .SetAbsoluteExpiration(TimeSpan.FromMinutes(10));
        // _cache.Set(CacheKeys.Categories, categories, cacheEntryOptions);

        var cacheEntryOptions = new MemoryCacheEntryOptions
        {
            SlidingExpiration = TimeSpan.FromMinutes(10),
            AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(30)
        };
        _cache.Set(CacheKeys.Categories, categories, cacheEntryOptions);
    }

    // Implement ICategoryService methods here
    public async Task<IEnumerable<Category>> GetCategoriesAsync()
    {
        // Try to get the categories from the cache
        if (_cache.TryGetValue(CacheKeys.Categories, out IEnumerable<Category>? categories))
        {
            _logger.LogInformation("Getting categories from cache");
            return categories ?? new List<Category>();
        }
        _logger.LogInformation("Fetching all categories from the database.");
        categories = await _dbContext.Categories.ToListAsync();
        await RefreshCacheAsync();
        // _logger.LogInformation("Fetching all categories from the database.");
        // categories = await _dbContext.Categories.ToListAsync();

        // // Cache the categories for 10 minutes
        // var cacheEntryOptions = new MemoryCacheEntryOptions()
        //     .SetAbsoluteExpiration(TimeSpan.FromMinutes(10));
        // _cache.Set(CacheKeys.Categories, categories, cacheEntryOptions);

        // Uncomment the following lines to set sliding and absolute expiration
        // var cacheEntryOptions = new MemoryCacheEntryOptions
        // {
        //     SlidingExpiration = TimeSpan.FromMinutes(10),
        //     AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(30)
        // };
        // _cache.Set(CacheKeys.Categories, categories, cacheEntryOptions);
        return categories;
    }
    public async Task<Category> GetCategoryByIdAsync(int id)
    {
        //The following code is not the best way to use the cache because it still queries the database even if the item is not found in the database
        // // Try to get the categories from the cache
        // if (_cache.TryGetValue($"{CacheKeys.Categories}:{id}", out Category? category))
        // {
        //     _logger.LogInformation($"Getting categories with id {id} from cache");
        //     return category;
        // }
        // _logger.LogInformation($"Fetching category with ID {id} from the database.");
        // category = await _dbContext.Categories.FindAsync(id);
        // if (category is not null)
        // { 
        //     _cache.Set($"{CacheKeys.Categories}:{id}", category);
        // }
        // return category;
        
        // The following code will set the cache as null if the item is not found in the database.
        // So the next time the item is requested, it will not query the database again
        var category = await _cache.GetOrCreateAsync($"{CacheKeys.Categories}:{id}", async entry =>
        {
            _logger.LogInformation($"Fetching category with ID {id} from the database.");
            var result = await _dbContext.Categories.FindAsync(id);
            return result;
        });
        return category;
    }
    public async Task<Category> AddCategoryAsync(Category category)
    {
        _dbContext.Categories.Add(category);
        await _dbContext.SaveChangesAsync();
        await RefreshCacheAsync();
        return category;
    }
    public async Task<Category> UpdateCategoryAsync(Category category)
    {
        var existingCategory = await _dbContext.Categories.FindAsync(category.Id);
        if (existingCategory == null)
        {
            throw new KeyNotFoundException($"Category with ID {category.Id} not found.");
        }
        _dbContext.Categories.Update(category);
        await _dbContext.SaveChangesAsync();
        await RefreshCacheAsync();
        return category;
    }
    public async Task<bool> DeleteCategoryAsync(int id)
    {
        var category = await _dbContext.Categories.FindAsync(id);
        if (category == null)
        {
            throw new KeyNotFoundException($"Category with ID {id} not found.");
        }
        _dbContext.Categories.Remove(category);
        await _dbContext.SaveChangesAsync();
        await RefreshCacheAsync();
        return true;
    }
}