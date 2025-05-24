using System.ComponentModel.DataAnnotations;
using FavoriteItemV2.Authentication;

namespace FavoriteItemV2.Models;

// Join entity between AppUser and Item
// This entity represents the many-to-many relationship between users and items
public class UserFavouriteItem
{
    [Key]
    public Guid Id { get; set; } = Guid.NewGuid(); // auto-generated values
    
    public string UserId { get; set; } = string.Empty;
    public AppUser User { get; set; } = null!;

    public int ItemId { get; set; }
    public Item Item { get; set; } = null!;
}
