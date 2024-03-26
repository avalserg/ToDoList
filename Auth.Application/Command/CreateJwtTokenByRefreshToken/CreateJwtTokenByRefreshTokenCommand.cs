using Auth.Application.Dtos;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Auth.Application.Command.CreateJwtTokenByRefreshToken
{
    public class CreateJwtTokenByRefreshTokenCommand : IRequest<JwtTokenDto>
    {
        public string JwtToken { get; set; } = default!;
        public string RefreshToken { get; set; } = default!;
        public DateTime Expires { get; set; }
    }
}
