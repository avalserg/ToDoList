using Common.Domain;
using Users.Service.Dto;

namespace Users.Service
{
    public interface IUserService
    {
        //IReadOnlyCollection<User> GetAllUsers(int? offset, string? labelFreeText, int? limit);
        Task<IReadOnlyCollection<GetUserDto>> GetAllUsersAsync(int? offset, string? labelFreeText, int? limit, bool? descending, CancellationToken cancellationToken);
        Task<GetUserDto?> GetUserByIdAsync(int id, CancellationToken cancellationToken);
        //User AddUser(CreateUserDto user);
        Task<GetUserDto?> AddUserAsync(CreateUserDto user, CancellationToken cancellationToken);
        //User? UpdateUser(UpdateUserDto newUser);
        Task<GetUserDto?> UpdateUserAsync(UpdateUserDto newUser, CancellationToken cancellationToken);
        //int Count(string? nameFreeText);
        Task<int> CountAsync(string? nameFreeText, CancellationToken cancellationToken);
        //bool RemoveUser(int id);
        Task<bool> RemoveUserAsync(int id, CancellationToken cancellationToken);


    }
}
