using Users.Service.Dto;

namespace Users.Service;

public interface IAuthUserService
{
    Task<string> GetJwtTokenAsync(AuthUserDto authUserDto);
}