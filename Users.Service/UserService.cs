using Common.Domain;
using Common.Repositories;
using Serilog;

namespace Users.Service
{
    public class UserService:IUserService
    {
        private readonly IBaseRepository<User> _userRepository;
       
        public UserService(IBaseRepository<User> userRepository)
        {
            _userRepository = userRepository;

            if (_userRepository.Count() > 0)
            {
                return;
            }
            //!!
            for (var i = 1; i < 4; i++)
            {
                _userRepository.Add(new User { Id = i, Name = $"User {i}" });
            }
        }

        public IReadOnlyCollection<User> GetAllUsers(int? offset, string? nameFreeText,  int? limit)
        {

            limit ??= 10;
            
            return _userRepository.GetList(
                offset, 
                limit,
                nameFreeText==null ? null: u=>u.Name.Contains(nameFreeText,StringComparison.InvariantCultureIgnoreCase),
                u=>u.Id);
        }

        public User? GetUserById(int id)
        {
            return _userRepository.GetSingleOrDefault(u=>u.Id==id);
        }

        public User AddUser(User user)
        {
            user.Id = _userRepository.Count() == 0 ? 1 : _userRepository.Count() + 1;
            return _userRepository.Add(user);
        }

        public User? UpdateUser(User newUser)
        {
            var user = _userRepository.GetSingleOrDefault(u=>u.Id==newUser.Id);
            if (user == null)
            {
                Log.Error($"User {newUser.Id} does not exist");
                return null;
            }

            Log.Information($"User with id={newUser.Id} was updated");
            return _userRepository.Update(newUser);
        }
        public int Count(string? nameFreeText)
        {
            return _userRepository.Count(nameFreeText == null
                ? null
                : t => t.Name.Contains(nameFreeText, StringComparison.InvariantCultureIgnoreCase));
        }
        public bool RemoveUser(int id)
        {
            var userRemove = _userRepository.GetSingleOrDefault(u => u.Id == id);
            if (userRemove == null)
            {
                Log.Error($"User with id={id} does not exist");
                return false;
            }

            Log.Information($"User with id={id} was deleted");
            return _userRepository.Delete(userRemove);
        }
    }
}
