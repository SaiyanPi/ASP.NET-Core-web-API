namespace FavoriteItemV2;

public static class CacheKeys
{
    //  public static string UserFavoriteItemCacheKey = "_UserFavoriteItems";
    public static string UserFavoriteItemCacheKey(string username) => $"UserFavoriteItemCacheKey:{username}";
}