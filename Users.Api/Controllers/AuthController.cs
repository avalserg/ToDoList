using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Users.Service;
using Users.Service.Dto;

namespace Users.Api.Controllers
{
    [Authorize]
    public class AuthController : ControllerBase
    {
        private readonly IAuthUserService _authUserService;
        private readonly IUserService _userService;

        public AuthController(IAuthUserService authUserService, IUserService userService)
        {
            _authUserService = authUserService;
            _userService = userService;
        }
        [AllowAnonymous]
        [HttpPost("CreateJwtToken")]
        public async Task<IActionResult> CreateJwtToken(AuthUserDto authUserDto)
        {
            var createdToken = await _authUserService.GetJwtTokenAsync(authUserDto);
            return Ok(createdToken);

        }

        
        [HttpGet("GetAuthUserInfo")]
        public async Task<IActionResult> GetAuthUserInfo(CancellationToken cancellationToken)
        {
          
            var currentUserId =User.FindFirst(ClaimTypes.NameIdentifier)!;
            var user = await _userService.GetUserByIdAsync(int.Parse(currentUserId.Value), cancellationToken);
            return Ok(user);

        }
        [Authorize(Roles = "Admin")]
        [HttpGet("Admin")]
        public async Task<IActionResult> GetAdminInfo(CancellationToken cancellationToken)
        {
          
            var currentUserId =User.FindFirst(ClaimTypes.NameIdentifier)!;
            var user = await _userService.GetUserByIdAsync(int.Parse(currentUserId.Value), cancellationToken);
            return Ok(user);

        }
    }
}
