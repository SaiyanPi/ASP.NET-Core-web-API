using System.Text.Json.Serialization;
using FavouriteItemApi.Authentication;

namespace FavouriteItemApi.Models;
public class ApplicationUser : AppUser
{
    // collection navigation property for Item class
    public ICollection<Item> Items { get; set; } = new List<Item>();

    // collection navigation property for UserFavouriteItem class
    [JsonIgnore]
    public ICollection<UserFavouriteItem> FavouriteItem { get; set; } = new List<UserFavouriteItem>();
}
