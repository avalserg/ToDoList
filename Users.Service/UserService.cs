using AutoMapper;
using Common.Domain;
using Common.Repositories;
using Common.Service.Exceptions;
using Serilog;
using Users.Service.Dto;
using Users.Service.Utils;

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
        }

        public IReadOnlyCollection<User> GetAllUsers(int? offset, string? nameFreeText,  int? limit)
        {

            limit ??= 10;
            
            return _userRepository.GetList(
                offset, 
                limit,
                nameFreeText==null ? null: u=>u.Login.Contains(nameFreeText),
                u=>u.Id);
        }
        public async Task<IReadOnlyCollection<GetUserDto>> GetAllUsersAsync(int? offset, string? nameFreeText, int? limit, bool? descending, CancellationToken cancellationToken)
        {

            limit ??= 10;
            
            return _mapper.Map<IReadOnlyCollection<GetUserDto>>( await _userRepository.GetListAsync(
                offset, 
                limit,
                nameFreeText==null ? null: u=>u.Login.Contains(nameFreeText),
                u=>u.Id,
                descending,
                cancellationToken));
        }

        public async Task<GetUserDto?> GetUserByIdAsync(int id, CancellationToken cancellationToken)
        {
           
            return _mapper.Map<GetUserDto>( await _userRepository.GetSingleOrDefaultAsync(u=>u.Id==id, cancellationToken));
        }

        public User AddUser(CreateUserDto user)
        {
            var userEntity = _mapper.Map<CreateUserDto, User>(user);

            return _userRepository.Add(userEntity);
        } 
        public async Task<GetUserDto?> AddUserAsync(CreateUserDto user, CancellationToken cancellationToken)
        {
            if (await _userRepository.GetSingleOrDefaultAsync(u=>u.Login==user.Login.Trim(),cancellationToken)!=null)
            {
                throw new BadRequestException("User login already exist");
            }
            var userEntity = new User()
            {
                Login = user.Login.Trim(),
                PasswordHash = PasswordHashUtil.Generate(user.Password),
                UserRoleId = 1
            };

            return _mapper.Map<GetUserDto>(await _userRepository.AddAsync(userEntity,cancellationToken));
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
        public async Task<GetUserDto?> UpdateUserAsync(UpdateUserDto newUser, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetSingleOrDefaultAsync(u=>u.Id==newUser.Id, cancellationToken);
            if (user == null)
            {
                Log.Error($"User {newUser.Id} does not exist");
                return null;
            }
            _mapper.Map(newUser, user);
            Log.Information($"User with id={newUser.Id} was updated");
            return _mapper.Map<GetUserDto>(await _userRepository.UpdateAsync(user, cancellationToken));
        }
        public int Count(string? nameFreeText)
        {
            return _userRepository.Count(nameFreeText == null
                ? null
                : t => t.Login.Contains(nameFreeText, StringComparison.InvariantCultureIgnoreCase));
        }  
        public async Task<int> CountAsync(string? nameFreeText, CancellationToken cancellationToken)
        {
            return await _userRepository.CountAsync(nameFreeText == null
                ? null
                : t => t.Login.Contains(nameFreeText, StringComparison.InvariantCultureIgnoreCase), cancellationToken);
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
        public async Task<bool> RemoveUserAsync(int id, CancellationToken cancellationToken)
        {
            var userRemove = await _userRepository.GetSingleOrDefaultAsync(u => u.Id == id, cancellationToken);
            if (userRemove == null)
            {
                Log.Error($"User with id={id} does not exist");
                return false;
            }

            Log.Information($"User with id={id} was deleted");
            return await _userRepository.DeleteAsync(userRemove, cancellationToken);
        }
    }
}
