using Microsoft.AspNetCore.Mvc;
using Users.Service;
using Users.Service.Dto;

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
        public async Task<IActionResult> GetAllUsersAsync(int? offset, string? nameFreeText, int? limit)
        {

            var users = await _userService.GetAllUsersAsync(offset, nameFreeText, limit);
            var countUsers = await _userService.CountAsync(nameFreeText);
            HttpContext.Response.Headers.Append("X-Total-Count", countUsers.ToString());
            return Ok(users);
        }
       
        [HttpGet("totalCount")]
        public async Task<IActionResult> GetCountUserAsync(string? labelFreeText)
        {

            var users = await _userService.CountAsync(labelFreeText);

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

        [HttpPost]
        public async Task<IActionResult> AddUserAsync(CreateUserDto newUser)
        {
            var user =await  _userService.AddUserAsync(newUser);

            return Created($"users/{user.Id}", user);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUserAsync(int id, UpdateUserDto updateUser)
        {
            updateUser.Id = id;
            var user = await _userService.UpdateUserAsync(updateUser);

            if (user == null)
            {
                return NotFound($"Запись с ID = {id} отсутствует");
            }

            return Ok(user);
        }

        [HttpDelete]
        public async Task<IActionResult> RemoveUserAsync([FromBody] int id)
        {
            var user = await _userService.RemoveUserAsync(id);

            if (!user)
            {
                return NotFound($"Запись с ID = {id} отсутствует");
            }

            return Ok($"Запись с ID = {id} удалена");
        }
    }
}
