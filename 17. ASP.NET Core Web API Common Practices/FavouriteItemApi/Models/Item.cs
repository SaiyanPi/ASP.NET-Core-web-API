using System.Text.Json.Serialization;

namespace FavouriteItemApi.Models;
public class Item
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    // collection navigation property for ApplicationUser class
    public ICollection<ApplicationUser> Users { get; set; } = new List<ApplicationUser>();
    // collection navigation property for UserFavouriteItem class
    [JsonIgnore]
    public ICollection<UserFavouriteItem> UserFavourite { get; set; } = new List<UserFavouriteItem>();
}
