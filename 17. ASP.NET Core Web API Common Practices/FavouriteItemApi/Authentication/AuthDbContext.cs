using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace FavouriteItemApi.Authentication;

public class AuthDbContext(DbContextOptions<AuthDbContext> options, IConfiguration configuration) 
: IdentityDbContext<AppUser>(options)
{
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        base.OnConfiguring(optionsBuilder);
        optionsBuilder.UseSqlServer(configuration.GetConnectionString("DefaultConnection"));
    }
}   