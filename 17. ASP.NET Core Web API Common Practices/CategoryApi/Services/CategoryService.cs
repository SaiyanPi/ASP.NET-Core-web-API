using CategoryApi.Data;
using Microsoft.EntityFrameworkCore;

namespace CategoryApi.Services;

public class CategoryService : ICategoryService
{
    private readonly CategoryDbContext _dbContext;
    private readonly ILogger<CategoryService> _logger;

    public CategoryService(CategoryDbContext dbContext, ILogger<CategoryService> logger)
    {
        _dbContext = dbContext;
        _logger = logger;
    }

    // Implement ICategoryService methods here
    public async Task<IEnumerable<Category>> GetCategoriesAsync()
    {
        _logger.LogInformation("Fetching all categories from the database.");
        return await _dbContext.Categories.ToListAsync();
    }
    public async Task<Category> GetCategoryByIdAsync(int id)
    {
        _logger.LogInformation($"Fetching category with ID {id} from the database.");
        var result = await _dbContext.Categories.FindAsync(id);
        if (result == null)
        {
            throw new KeyNotFoundException($"Category with ID {id} not found.");
        }
        return result;
    }
    public async Task<Category> AddCategoryAsync(Category category)
    {
        _dbContext.Categories.Add(category);
        await _dbContext.SaveChangesAsync();
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
        return true;
    }
}