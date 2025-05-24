using System.Text.Json.Serialization;
using FavoriteItemV2.Authentication;

namespace FavoriteItemV2.Models;
public class Item
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    // collection navigation property for AppUser class
    public ICollection<AppUser> Users { get; set; } = new List<AppUser>();
    // collection navigation property for UserFavouriteItem class
    [JsonIgnore]
    public ICollection<UserFavouriteItem> UserFavourite { get; set; } = new List<UserFavouriteItem>();
}
