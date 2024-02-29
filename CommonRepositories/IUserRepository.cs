using Common.Domain;

namespace Common.Repositories
{
    public interface IUserRepository
    {
        IReadOnlyCollection<User> GetAllUser(int? offset, string? labelFreeText);
        User? GetUserById(int id);
        User AddUser(User toDo);
        User? UpdateUser(User newUser);
        bool RemoveUser(int id);
    }
}
