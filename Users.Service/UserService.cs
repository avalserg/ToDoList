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

            limit ??= 10;
            
            return _userRepository.GetAllUser(offset, labelFreeText).Take(limit.Value).ToList();
        }

        public User? GetUserById(int id)
        {
            return _userRepository.GetUserById(id);
        }

        public User AddUser(User user)
        {
            return _userRepository.AddUser(user);
        }

        public User? UpdateUser(User newUser)
        {
            var todo = _userRepository.GetUserById(newUser.Id);
            if (todo == null)
            {
                return null;
            }
          
            return _userRepository.UpdateUser(newUser);
        }

        public bool RemoveUser(int id)
        {
            var todoToRemove = _userRepository.GetUserById(id);
            if (todoToRemove == null)
            {
                return false;
            }
            return _userRepository.RemoveUser(id);
        }
    }
}
