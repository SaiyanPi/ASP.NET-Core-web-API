using System.Text.Json.Serialization;
using FavoriteItemV2.Models;
using Microsoft.AspNetCore.Identity;

namespace FavoriteItemV2.Authentication;
public class AppUser : IdentityUser
{
    // collection navigation property for Item class
    public ICollection<Item> Items { get; set; } = new List<Item>();

    // collection navigation property for UserFavouriteItem class
    [JsonIgnore]
    public ICollection<UserFavouriteItem> FavouriteItem { get; set; } = new List<UserFavouriteItem>();
}
