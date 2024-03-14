using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Users.Service;
using Users.Service.Dto;

namespace Users.Api.Controllers
{
    [Authorize]
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
        public async Task<IActionResult> GetAllUsersAsync(int? offset, string? nameFreeText, int? limit, bool? descending, CancellationToken cancellationToken)
        {

            var users = await _userService.GetAllUsersAsync(offset, nameFreeText, limit, descending,cancellationToken);
            var countUsers = await _userService.CountAsync(nameFreeText, cancellationToken);
            HttpContext.Response.Headers.Append("X-Total-Count", countUsers.ToString());
            return Ok(users);
        }
       
        [HttpGet("totalCount")]
        public async Task<IActionResult> GetCountUserAsync(string? labelFreeText, CancellationToken cancellationToken)
        {

            var users = await _userService.CountAsync(labelFreeText, cancellationToken);

            return Ok(users);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetUserByIdAsync(int id, CancellationToken cancellationToken)
        {

            var user = await _userService.GetUserByIdAsync(id, cancellationToken);

            if (user == null)
            {
                return NotFound($"Пользователь с ID = {id} отсутствует");
            }

            return Ok(user);
        }
        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> AddUserAsync(CreateUserDto newUser, CancellationToken cancellationToken)
        {
            var user =await  _userService.AddUserAsync(newUser, cancellationToken);

            return Created($"users/{user.Id}", user);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUserAsync(int id, UpdateUserDto updateUser, CancellationToken cancellationToken)
        {
            updateUser.Id = id;
            var user = await _userService.UpdateUserAsync(updateUser, cancellationToken);

            if (user == null)
            {
                return NotFound($"Запись с ID = {id} отсутствует");
            }

            return Ok(user);
        }

        [HttpDelete]
        public async Task<IActionResult> RemoveUserAsync([FromBody] int id, CancellationToken cancellationToken)
        {
            var user = await _userService.RemoveUserAsync(id, cancellationToken);

            if (!user)
            {
                return NotFound($"Запись с ID = {id} отсутствует");
            }

            return Ok($"Запись с ID = {id} удалена");
        }
    }
}
