using AutoMapper;
using Common.Domain;
using Common.Repositories;
using Serilog;
using Users.Service.Dto;

namespace Users.Service
{
    public class UserService:IUserService
    {
        private readonly IBaseRepository<User> _userRepository;
        private readonly IMapper _mapper;

        public UserService(IBaseRepository<User> userRepository, IMapper mapper)
        {
            _userRepository = userRepository;
            _mapper = mapper;

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
                nameFreeText==null ? null: u=>u.Name.Contains(nameFreeText),
                u=>u.Id);
        }

        public async Task<User?> GetUserByIdAsync(int id, CancellationToken cancellationToken)
        {
           var user =  await _userRepository.GetSingleOrDefaultAsync(u => u.Id == id, cancellationToken);

            return await _userRepository.GetSingleOrDefaultAsync(u=>u.Id==id, cancellationToken);
        }

        public User AddUser(CreateUserDto user)
        {
            var userEntity = _mapper.Map<CreateUserDto, User>(user);

            return _userRepository.Add(userEntity);
        }

        public User? UpdateUser(UpdateUserDto newUser)
        {
            var user = _userRepository.GetSingleOrDefault(u=>u.Id==newUser.Id);
            if (user == null)
            {
                Log.Error($"User {newUser.Id} does not exist");
                return null;
            }
            _mapper.Map(newUser, user);
            Log.Information($"User with id={newUser.Id} was updated");
            return _userRepository.Update(user);
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
