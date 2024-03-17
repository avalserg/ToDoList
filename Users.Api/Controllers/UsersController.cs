using System.Security.Claims;
using Authorization.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Users.Service;
using Users.Service.Dto;
using Users.Service.Dtos;

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
        [Authorize]
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
        [AllowAnonymous]
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
            var updatedUser = await _userService.GetUserByIdAsync(id, cancellationToken);
            if (updatedUser == null)
            {
                return NotFound($"Пользователь с ID = {id} отсутствует");
            }

            var currentLoggedInUserId = User.FindFirst(ClaimTypes.NameIdentifier);
            var currentLoggedInUser = await _userService.GetUserByIdAsync(int.Parse(currentLoggedInUserId!.Value), cancellationToken);
           
            if (updatedUser.Login == currentLoggedInUser!.Login||
                currentLoggedInUser.Roles.Any(r=>r.ApplicationUserRole.Name=="Admin"))
            {
                updatedUser = await _userService.UpdateUserAsync(updateUser, cancellationToken);
            }
            else
            {
                return BadRequest($"User {updatedUser.Login} with {id} не может быть изменен юзером {currentLoggedInUser.Login}");
            }
            return Ok(updatedUser);
        }
        
        [HttpPut("{id}/Password")]
        public async Task<IActionResult> UpdateUserPasswordAsync(int id, UpdateUserPasswordDto updateUserPassword, CancellationToken cancellationToken)
        {
            updateUserPassword.Id = id;
            var updatedUser = await _userService.GetUserByIdAsync(id, cancellationToken);
            if (updatedUser == null)
            {
                return NotFound($"Пользователь с ID = {id} отсутствует");
            }

            var currentLoggedInUserId = User.FindFirst(ClaimTypes.NameIdentifier);
            var currentLoggedInUser = await _userService.GetUserByIdAsync(int.Parse(currentLoggedInUserId!.Value), cancellationToken);

            if (updatedUser.Login == currentLoggedInUser!.Login ||
                currentLoggedInUser.Roles.Any(r => r.ApplicationUserRole.Name == "Admin"))
            {
                updatedUser = await _userService.UpdateUserPasswordAsync(updateUserPassword, cancellationToken);
            }
            else
            {
                return BadRequest($"Пароль пользователя {updatedUser.Login} with {id} не может быть изменен юзером {currentLoggedInUser.Login}");
            }
            return Ok(updatedUser);
            
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
