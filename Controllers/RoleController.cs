using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using JWTSwagger.Authentication;
using Swashbuckle.AspNetCore.Annotations;

namespace JWTSwagger.Controllers
{
  [Produces("application/json")]
  [Route("api/[controller]")]
  [ApiController]
  public class RoleController(RoleManager<IdentityRole> roleManager) : Controller
  {
    private readonly RoleManager<IdentityRole> _roleManager = roleManager;

    [HttpGet]
    [SwaggerOperation(summary: "Return all roles", null)]
    [SwaggerResponse(200, "Success", typeof(RoleDTO))]
    public IActionResult Get() => Ok(_roleManager.Roles.OrderBy(r => r.Name).Select(r => new RoleDTO { Id = r.Id, Name = r.Name }));
  }
}
