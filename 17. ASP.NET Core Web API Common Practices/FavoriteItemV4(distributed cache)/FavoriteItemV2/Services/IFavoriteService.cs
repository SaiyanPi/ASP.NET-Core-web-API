using FavoriteItemV2.Models;

namespace FavoriteItemV2.Services;
public interface IFavoriteService
{
    Task<IEnumerable<Item>> GetUserFavoritesAsync(string username);
    Task<bool> AddToFavoritesAsync(string username, int itemId);
    Task<bool> RemoveFromFavoritesAsync(string username, int itemId);
}