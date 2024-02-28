using Common.Domain;
using Common.Repositories;

namespace Users.Service
{
    public class UserService:IUserService
    {
        private readonly IUserRepository _userRepository;

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }
        public IReadOnlyCollection<User> GetAllUsers(int? offset, string? labelFreeText, int? limit)
        {
            return _userRepository.GetAllUser(offset, labelFreeText, limit);
        }

        public User? GetUserById(int id)
        {
            return _userRepository.GetUserById(id);
        }

        public User AddUser(User user)
        {
            return _userRepository.AddUser(user);
        }

        public User? UpdateUser(int id, User newUser)
        {
            var todo = _userRepository.GetUserById(id);
            if (todo == null)
            {
                return null;
            }
          
            return _userRepository.UpdateUser(id, newUser);
        }

        public User RemoveUser(int id)
        {
            var todoToRemove = _userRepository.GetUserById(id);
            if (todoToRemove == null)
            {
                return null;
            }
            return _userRepository.RemoveUser(id);
        }
    }
}
