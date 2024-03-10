using Common.Domain;

namespace Users.Service
{
    public interface IUserService
    {
        IReadOnlyCollection<User> GetAllUsers(int? offset, string? labelFreeText, int? limit);
        Task<User?> GetUserById(int id);
        User AddUser(User user);
        User? UpdateUser(User newUser);
        int Count(string? nameFreeText);
        bool RemoveUser(int id);
      
    }
}
