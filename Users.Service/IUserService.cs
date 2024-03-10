using Common.Domain;
using Users.Service.Dto;

namespace Users.Service
{
    public interface IUserService
    {
        IReadOnlyCollection<User> GetAllUsers(int? offset, string? labelFreeText, int? limit);
        Task<User?> GetUserByIdAsync(int id, CancellationToken cancellationToken);
        User AddUser(CreateUserDto user);
        User? UpdateUser(UpdateUserDto newUser);
        int Count(string? nameFreeText);
        bool RemoveUser(int id);
      
    }
}
