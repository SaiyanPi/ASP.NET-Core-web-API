using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CategoryApi.Data;
using CategoryApi.Services;

namespace CategoryApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private readonly ICategoryService _categoryService;
        public CategoriesController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        // GET: api/Categories
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Category>>> GetCategories()
        {
            var categories = await _categoryService.GetCategoriesAsync();
            return Ok(categories);
        }

        // GET: api/Categories/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Category>> GetCategory(int id)
        {
            var category = await _categoryService.GetCategoryByIdAsync(id);

            if (category == null)
            {
                return NotFound();
            }

            return Ok(category);
        }

        // POST: api/Categories
        [HttpPost]
        public async Task<ActionResult<Category>> PostCategory(Category category)
        {
            var createdCategory = await _categoryService.AddCategoryAsync(category);
            return CreatedAtAction(nameof(GetCategory), new { id = createdCategory.Id }, createdCategory);
        }

        // PUT: api/Categories/5
        [HttpPut("{id}")]
        public async Task<ActionResult<Category?>> UpdateCategory(Category category)
        {
            //     var existingCategory = await _categoryService.GetCategoryByIdAsync(id);
            //     if (existingCategory == null)
            //     {
            //         return NotFound();
            //     }
            //    return await _categoryService.UpdateCategoryAsync(category);
            if (!await CategoryExists(category.Id))
            {
                return NotFound();
            }
            return await _categoryService.UpdateCategoryAsync(category);
        }

        // DELETE: api/Categories/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<bool>> DeleteCategory(int id)
        {
            // try
            // {
            //     await _categoryService.DeleteCategoryAsync(id);
            // }
            // catch (KeyNotFoundException)
            // {
            //     return NotFound();
            // }
            // return NoContent();
            if (!await CategoryExists(id))
            {
                return NotFound();
            }
            await _categoryService.DeleteCategoryAsync(id);
            return NoContent();
        }

        private async Task<bool> CategoryExists(int id)
        {
            var category = await _categoryService.GetCategoryByIdAsync(id);
            return category != null;
        }
    }
}
