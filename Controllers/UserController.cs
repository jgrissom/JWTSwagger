using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using JWTSwagger.Authentication;
using Swashbuckle.AspNetCore.Annotations;

namespace JWTSwagger.Controllers
{
  [Produces("application/json")]
  [Route("api/[controller]")]
  [ApiController]
  public class UserController(UserManager<ApplicationUser> userManager) : ControllerBase
  {
    // access UserManager w/ dependecy injection
    private readonly UserManager<ApplicationUser> _userManager = userManager;

    [HttpGet]
    [SwaggerOperation(summary: "Return all users", null)]
    [SwaggerResponse(200, "Success", typeof(UserDTO))]
    public IActionResult Get() => Ok(_userManager.Users.Select(u => 
      new UserDTO { 
        Id = u.Id, 
        Email = u.Email, 
        UserName = u.UserName 
      }).OrderBy(u => u.UserName));

    [HttpPost]
    [Route("register")]
    [SwaggerOperation(summary: "User registration", null)]
    [SwaggerResponse(204, "User registered", null)]
    public async Task<IActionResult> Register([FromBody] UserRegister model)
    {
      // check for existence of user
      var userExists = await _userManager.FindByEmailAsync(model.Email!);
      if (userExists != null) // duplicate email
        return Conflict(new { message = $"{model.Email} is a duplicate Email" });
      userExists = await _userManager.FindByNameAsync(model.Username!);
      if (userExists != null) // duplicate username
        return Conflict(new { message = $"{model.Username} is a duplicate Username" });

      ApplicationUser user = new ApplicationUser()
      {
        Email = model.Email,
        SecurityStamp = Guid.NewGuid().ToString(),
        UserName = model.Username
      };
      var result = await _userManager.CreateAsync(user, model.Password!);
      if (!result.Succeeded)
        return StatusCode(StatusCodes.Status500InternalServerError);

      return NoContent();
    }
  }
}
