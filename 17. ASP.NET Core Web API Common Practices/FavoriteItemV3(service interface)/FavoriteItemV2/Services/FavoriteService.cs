// using FavoriteItemV2.Data;
// using FavoriteItemV2.Models;

// namespace FavoriteItemV2.Services;

// public class FavoriteService(ApplicationDbContext context, ILogger<FavoriteService> logger)
// : IFavoriteService
// {
//     public async Task<IEnumerable<Item>> GetUserFavoritesAsync(string username)
//     {
//         var user = await context.Users
//             .Include(u => u.FavoriteItems)
//             .ThenInclude(f => f.Item)
//             .FirstOrDefaultAsync(u => u.UserName == username);

//         if (user == null) return Enumerable.Empty<Item>();

//         return user.FavoriteItems.Select(f => f.Item);
//     }
// }
