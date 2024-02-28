using Microsoft.AspNetCore.Mvc;
using Users.Service;

namespace Users.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UsersController : ControllerBase
    {
      
        private readonly IUserService _userService;

        public UsersController( IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet]
        public IActionResult GetAllUsers(int? offset, string? nameFreeText, int? limit)
        {

            var users = _userService.GetAllUsers(offset, nameFreeText, limit);

            if (!users.Any())
            {
                return NoContent();
            }

            return Ok(users);
        }
    }
}
