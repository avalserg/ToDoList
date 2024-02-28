using Common.Domain;

namespace Users.Service
{
    public interface IUserService
    {
        IReadOnlyCollection<User> GetAllUsers(int? offset, string? labelFreeText, int? limit = 10);
        User? GetUserById(int id);
        User AddUser(User toDo);
        User? UpdateUser(int id, User newUser);
        User RemoveUser(int id);
    }
}
