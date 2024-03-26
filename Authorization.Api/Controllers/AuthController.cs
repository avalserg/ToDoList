using Auth.Application;
using Auth.Application.Command.CreateJwtToken;
using Auth.Application.Command.CreateJwtTokenByRefreshToken;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Authorization.Api.Controllers
{
    [Authorize]
    public class AuthController : ControllerBase
    {
        
        private readonly IMediator _mediator;


        public AuthController(IMediator mediator )
        {
           _mediator = mediator;
        }
        [AllowAnonymous]
        [HttpPost("CreateJwtToken")]
        public async Task<IActionResult> CreateJwtTokenAsync(CreateJwtTokenCommand createJwtTokenCommand , CancellationToken cancellationToken)
        {
            var createdToken = await _mediator.Send(createJwtTokenCommand, cancellationToken);
            return Ok(createdToken);

        }
        [AllowAnonymous]
        [HttpPost("CreateJwtTokenByRefreshToken")]
        public async Task<IActionResult> CreateJwtTokenByRefreshTokenAsync(string refreshToken, CancellationToken cancellationToken)
        {
            var createdToken = await _mediator.Send(new CreateJwtTokenByRefreshTokenCommand(){RefreshToken = refreshToken}, cancellationToken);
            return Ok(createdToken);

        }

    }
}
