using Common.Domain;
using Users.Service.Dto;
using Users.Service.Dtos;

namespace Users.Service
{
    public interface IUserService
    {
      
        Task<IReadOnlyCollection<GetUserDto>> GetAllUsersAsync(int? offset, string? labelFreeText, int? limit, bool? descending, CancellationToken cancellationToken);
        Task<GetUserDto?> GetUserByIdAsync(int id, CancellationToken cancellationToken);
       
        Task<GetUserDto?> AddUserAsync(CreateUserDto user, CancellationToken cancellationToken);
      
        Task<GetUserDto?> UpdateUserAsync(UpdateUserDto newUser, CancellationToken cancellationToken);
        
        Task<int> CountAsync(string? nameFreeText, CancellationToken cancellationToken);
       
        Task<bool> RemoveUserAsync(int id, CancellationToken cancellationToken);

        Task<GetUserDto?> UpdateUserPasswordAsync(UpdateUserPasswordDto newUserPassword,
            CancellationToken cancellationToken);
    }
}
