using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Auth.Application.Dtos;
using Common.Application.Abstractions.Persistence;
using Common.Application.Exceptions;
using Common.Application.Utils;
using Common.Domain;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace Auth.Application.Command.CreateJwtToken
{
    public class CreateJwtTokenCommandHandler:IRequestHandler<CreateJwtTokenCommand, JwtTokenDto>
    {
        private readonly IBaseRepository<ApplicationUser> _userRepository;
        private readonly IBaseRepository<RefreshToken> _refreshTokensRepository;
        private readonly IConfiguration _configuration;

        public CreateJwtTokenCommandHandler(
            IBaseRepository<ApplicationUser> userRepository,
            IBaseRepository<RefreshToken> refreshTokensRepository,
            IConfiguration configuration
            )
        {
            _userRepository = userRepository;
            _refreshTokensRepository = refreshTokensRepository;
            _configuration = configuration;
        }
        public async Task<JwtTokenDto> Handle(CreateJwtTokenCommand request, CancellationToken cancellationToken)
        {
            var jwtOptions = _configuration.GetSection(nameof(JwtOptions)).Get<JwtOptions>();
            var user = await _userRepository.GetSingleOrDefaultAsync(u => u.Login == request.Login.Trim(), cancellationToken);
            if (user == null)
            {
                throw new NotFoundException($"User with login {request.Login} don't exist");
            }


            //var role = await _userRoleRepository.GetSingleOrDefaultAsync(r => r.Id == userRepository.UserRoleId, cancellationToken);

            if (!PasswordHashUtil.VerifyPassword(request.Password, user.PasswordHash))
            {
                throw new ForbiddenExceptions();
            }
            var claims = new List<Claim>{
                new(ClaimTypes.Name, request.Login),
                new(ClaimTypes.NameIdentifier, user.Id.ToString()),

            };
            // Add all roles userRepository to token
            claims.AddRange(user.Roles.Select(role => new Claim(ClaimTypes.Role, role.ApplicationUserRole.Name)));

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

            var refreshToken =
                await _refreshTokensRepository.AddAsync(new RefreshToken() { ApplicationUserId = user.Id }, cancellationToken);
            return new JwtTokenDto()
            {
                JwtToken = tokenValue,
                RefreshToken = refreshToken!.Id,
                Expires = dataExpires
            };
        }
    }
}
