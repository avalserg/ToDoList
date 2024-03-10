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
        public IActionResult GetAllUsers(int? offset, string? nameFreeText, int? limit)
        {

            var users = _userService.GetAllUsers(offset, nameFreeText, limit);
            var countUsers = _userService.Count(nameFreeText);
            HttpContext.Response.Headers.Append("X-Total-Count", countUsers.ToString());
            return Ok(users);
        }
       
        [HttpGet("totalCount")]
        public IActionResult GetCountUser(string? labelFreeText)
        {

            var users = _userService.Count(labelFreeText);

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
        public IActionResult AddUserAsync(CreateUserDto newUser)
        {
            var user =  _userService.AddUser(newUser);

            return Created($"users/{user.Id}", user);
        }

        [HttpPut("{id}")]
        public IActionResult UpdateUser(int id, UpdateUserDto updateUser)
        {
            updateUser.Id = id;
            var user = _userService.UpdateUser(updateUser);

            if (user == null)
            {
                return NotFound($"Запись с ID = {id} отсутствует");
            }

            return Ok(user);
        }

        [HttpDelete]
        public IActionResult RemoveUser([FromBody] int id)
        {
            var user = _userService.RemoveUser(id);

            if (!user)
            {
                return NotFound($"Запись с ID = {id} отсутствует");
            }

            return Ok($"Запись с ID = {id} удалена");
        }
    }
}
