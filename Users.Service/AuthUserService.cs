using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Common.Domain;
using Common.Repositories;
using Common.Service.Exceptions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Users.Service.Dto;
using Users.Service.Utils;

namespace Users.Service
{
    public class AuthUserService : IAuthUserService
    {
        private readonly IBaseRepository<User> _user;
        private readonly IConfiguration _configuration;
        private readonly IBaseRepository<UserRole> _userRoleRepository;


        public AuthUserService(IBaseRepository<User> user, IOptions<JwtOptions> options, IConfiguration configuration, IBaseRepository<UserRole> userRoleRepository)
        {
            _user = user;
            _configuration = configuration;
            _userRoleRepository = userRoleRepository;
        }

        public async Task<string> GetJwtTokenAsync(AuthUserDto authUserDto)
        {
            var jwtOptions = _configuration.GetSection(nameof(JwtOptions)).Get<JwtOptions>();
            var user = await _user.GetSingleOrDefaultAsync(u => u.Login == authUserDto.Login.Trim());
            if (user == null)
            {
                throw new NotFoundException($"User with login {authUserDto.Login} don't exist");
            }

            var role = await _userRoleRepository.GetSingleOrDefaultAsync(r => r.Id == user.UserRoleId);
            
            if (!PasswordHashUtil.Verify(authUserDto.Password, user.PasswordHash))
            {
                throw new ForbiddenExceptions();
            }
            var claims =new List<Claim>{
                new(ClaimTypes.Name, authUserDto.Login),
                new(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new(ClaimTypes.Role, role!.Name),
                
            };
            var signingCredentials = new SigningCredentials(
                new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtOptions.SecretKey)),
                SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                claims: claims,
                issuer: jwtOptions.Issuer,
                audience:jwtOptions.Audience,
                expires: DateTime.UtcNow.AddHours(jwtOptions.ExpiresHours),
                signingCredentials: signingCredentials);

            //token to string
            var tokenValue = new JwtSecurityTokenHandler().WriteToken(token);

            return tokenValue;
        }
    }
}
