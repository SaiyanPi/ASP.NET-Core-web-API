using FavouriteItemApi.Authentication;
using FavouriteItemApi.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace FavouriteItemApi.Controllers;

[ApiController]
[Route("[controller]")]
public class ItemController(ApplicationDbContext context, UserManager<AppUser> userManager)
: ControllerBase
{
   
}