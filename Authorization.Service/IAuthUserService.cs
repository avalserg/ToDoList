using Authorization.Service.Dtos;

namespace Authorization.Service;

public interface IAuthUserService
{
    Task<JwtTokenDto> GetJwtTokenAsync(AuthUserDto authUserDto, CancellationToken cancellationToken);
    Task<JwtTokenDto> GetJwtTokenByRefreshTokenAsync(string refreshToken, CancellationToken cancellationToken);
}