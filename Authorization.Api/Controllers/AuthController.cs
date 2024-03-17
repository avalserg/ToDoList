using Authorization.Service;
using Authorization.Service.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Authorization.Api.Controllers
{
    [Authorize]
    public class AuthController : ControllerBase
    {
        private readonly IAuthUserService _authUserService;
        

        public AuthController(IAuthUserService authUserService)
        {
            _authUserService = authUserService;
           
        }
        [AllowAnonymous]
        [HttpPost("CreateJwtToken")]
        public async Task<IActionResult> CreateJwtToken(AuthUserDto authUserDto, CancellationToken cancellationToken)
        {
            var createdToken = await _authUserService.GetJwtTokenAsync(authUserDto, cancellationToken);
            return Ok(createdToken);

        }
        [AllowAnonymous]
        [HttpPost("CreateJwtTokenByRefreshToken")]
        public async Task<IActionResult> CreateJwtTokenByRefreshToken(string refreshToken, CancellationToken cancellationToken)
        {
            var createdToken = await _authUserService.GetJwtTokenByRefreshTokenAsync(refreshToken, cancellationToken);
            return Ok(createdToken);

        }

    }
}
