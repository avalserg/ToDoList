using Auth.Application.Dtos;
using MediatR;

namespace Auth.Application.Command.CreateJwtToken
{
    public class CreateJwtTokenCommand:IRequest<JwtTokenDto>
    {
        public string Password { get; set; } = default!;
        public string Login { get; set; } = default!;
    }
}
