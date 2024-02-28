using Common.Domain;

namespace Common.Repositories
{
    public interface IUserRepository
    {
        IReadOnlyCollection<User> GetAllUser(int? offset, string? labelFreeText, int? limit = 10);
        User? GetUserById(int id);
        User AddUser(User toDo);
        User? UpdateUser(int id, User newUser);
        User RemoveUser(int id);
    }
}
