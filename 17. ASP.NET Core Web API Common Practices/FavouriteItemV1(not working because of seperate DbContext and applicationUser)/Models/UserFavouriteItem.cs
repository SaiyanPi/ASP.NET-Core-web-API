namespace FavouriteItemApi.Models;

public class UserFavouriteItem
{
    public string UserId { get; set; } = string.Empty;
    public ApplicationUser User { get; set; } = null!;

    public int ItemId { get; set; }
    public Item Item { get; set; } = null!;
}
