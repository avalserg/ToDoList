using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Authorization.Service.Dtos;
using Common.Domain;
using Common.Repositories;
using Common.Service.Exceptions;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Users.Service;
using Users.Service.Utils;

namespace Authorization.Service
{
    public class AuthUserService : IAuthUserService
    {
        private readonly IBaseRepository<ApplicationUser> _userRepository;
        private readonly IBaseRepository<RefreshToken> _refreshTokensRepository;
        private readonly IConfiguration _configuration;


        public AuthUserService(
            IBaseRepository<ApplicationUser> userRepository,
            IBaseRepository<RefreshToken> refreshTokensRepository,
            //IOptions<JwtOptions> options, 
            IConfiguration configuration, 
            IBaseRepository<ApplicationUserApplicationRole> userRoleRepository)
        {
            _userRepository = userRepository;
            _refreshTokensRepository = refreshTokensRepository;
            _configuration = configuration;
        }

        public async Task<JwtTokenDto> GetJwtTokenAsync(AuthUserDto authUserDto, CancellationToken cancellationToken)
        {
            var jwtOptions = _configuration.GetSection(nameof(JwtOptions)).Get<JwtOptions>();
            var user = await _userRepository.GetSingleOrDefaultAsync(u => u.Login == authUserDto.Login.Trim(), cancellationToken);
            if (user == null)
            {
                throw new NotFoundException($"User with login {authUserDto.Login} don't exist");
            }

            
            //var role = await _userRoleRepository.GetSingleOrDefaultAsync(r => r.Id == userRepository.UserRoleId, cancellationToken);
            
            if (!PasswordHashUtil.VerifyPassword(authUserDto.Password, user.PasswordHash))
            {
                throw new ForbiddenExceptions();
            }
            var claims =new List<Claim>{
                new(ClaimTypes.Name, authUserDto.Login),
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
                audience:jwtOptions.Audience,
                expires: dataExpires,
                signingCredentials: signingCredentials);

            //token to string
            var tokenValue = new JwtSecurityTokenHandler().WriteToken(token);

            var refreshToken =
                await _refreshTokensRepository.AddAsync(new RefreshToken() { ApplicationUserId = user.Id },cancellationToken);
            return new JwtTokenDto()
            {
                JwtToken = tokenValue,
                RefreshToken = refreshToken!.Id,
                Expires = dataExpires
            };
           // return tokenValue;
        }

        public async Task<JwtTokenDto> GetJwtTokenByRefreshTokenAsync(string refreshToken, CancellationToken cancellationToken)
        {
            var refreshTokenFromDb =
                await _refreshTokensRepository.GetSingleOrDefaultAsync(e => e.Id == refreshToken, cancellationToken);
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
