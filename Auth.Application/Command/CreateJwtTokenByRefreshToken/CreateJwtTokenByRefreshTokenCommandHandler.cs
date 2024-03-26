using Auth.Application.Command.CreateJwtToken;
using Auth.Application.Dtos;
using MediatR;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Common.Application.Abstractions.Persistence;
using Common.Application.Exceptions;
using Common.Domain;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace Auth.Application.Command.CreateJwtTokenByRefreshToken
{
    public class CreateJwtTokenByRefreshTokenCommandHandler : IRequestHandler<CreateJwtTokenByRefreshTokenCommand, JwtTokenDto>
    {
        private readonly IBaseRepository<RefreshToken> _refreshTokensRepository;
        private readonly IBaseRepository<ApplicationUser> _userRepository;
        private readonly IConfiguration _configuration;

        public CreateJwtTokenByRefreshTokenCommandHandler(
            IBaseRepository<RefreshToken> refreshTokensRepository,
            IBaseRepository<ApplicationUser> userRepository,
            IConfiguration configuration
            )
        {
            _refreshTokensRepository = refreshTokensRepository;
            _userRepository = userRepository;
            _configuration = configuration;
        }
        public async Task<JwtTokenDto> Handle(CreateJwtTokenByRefreshTokenCommand request, CancellationToken cancellationToken)
        {
            var refreshTokenFromDb =
                await _refreshTokensRepository.GetSingleOrDefaultAsync(e => e.Id == request.RefreshToken, cancellationToken);
            if (refreshTokenFromDb is null)
            {
                throw new ForbiddenExceptions();
            }
            var user = await _userRepository.GetSingleAsync(u => u.Id == refreshTokenFromDb.ApplicationUserId, cancellationToken);

            var claims = new List<Claim>{
                new(ClaimTypes.Name, user.Login),
                new(ClaimTypes.NameIdentifier, user.Id.ToString()),

            };
            // Add all roles userRepository to token
            claims.AddRange(user.Roles.Select(role => new Claim(ClaimTypes.Role, role.ApplicationUserRole.Name)));

            var jwtOptions = _configuration.GetSection(nameof(JwtOptions)).Get<JwtOptions>();

            var signingCredentials = new SigningCredentials(
                new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtOptions.SecretKey)),
                SecurityAlgorithms.HmacSha256);
            var dataExpires = DateTime.UtcNow.AddHours(jwtOptions.ExpiresHours);

            var token = new JwtSecurityToken(
                claims: claims,
                issuer: jwtOptions.Issuer,
                audience: jwtOptions.Audience,
                expires: dataExpires,
                signingCredentials: signingCredentials);

            //token to string
            var tokenValue = new JwtSecurityTokenHandler().WriteToken(token);

            return new JwtTokenDto()
            {
                JwtToken = tokenValue,
                RefreshToken = refreshTokenFromDb.Id,
                Expires = dataExpires
            };
        }
    }
}
