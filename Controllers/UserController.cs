using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using JWTSwagger.Authentication;

namespace JWTSwagger.Controllers
{
  [Route("api/[controller]")]
  [ApiController]
  public class UserController(UserManager<ApplicationUser> userManager) : ControllerBase
  {
    // access UserManager w/ dependecy injection
    private readonly UserManager<ApplicationUser> _userManager = userManager;

    [HttpGet]
    public IActionResult Get() => Ok(_userManager.Users);
  }
}
