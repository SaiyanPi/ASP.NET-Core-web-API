using FavoriteItemV2.Models;

namespace FavoriteItemV2.Services;
public interface IItemService
{
    Task<IEnumerable<Item>> GetItemsAsync();
    Task<Item?> GetItemByIdAsync(int id);
    Task<Item> CreateItemAsync(Item item);
    Task<bool> UpdateItemAsync(int id, Item item);
    Task<bool> DeleteItemAsync(int id);


    Task<IEnumerable<Item>> GetUserFavoritesAsync(string username);
    Task<string> AddToFavoritesAsync(string username, int itemId);
    Task<string> RemoveFromFavoritesAsync(string username, int itemId);
}