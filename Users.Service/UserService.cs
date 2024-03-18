using AutoMapper;
using Common.Domain;
using Common.Repositories;
using Common.Service;
using Common.Service.Exceptions;
using Serilog;
using Users.Service.Dtos;
using Users.Service.Utils;

namespace Users.Service
{
    public class UserService:IUserService
    {
        private readonly IBaseRepository<ApplicationUser> _userRepository;
        private readonly IBaseRepository<ApplicationUserRole> _userRoles;
        private readonly IMapper _mapper;
        private readonly ICurrentUserSerice _currentUserSerice;

        public UserService(IBaseRepository<ApplicationUser> userRepository,IBaseRepository<ApplicationUserRole> userRoles ,IMapper mapper, ICurrentUserSerice currentUserSerice)
        {
            _userRepository = userRepository;
            _userRoles = userRoles;
            _mapper = mapper;
            _currentUserSerice = currentUserSerice;
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
        
        public async Task<GetUserDto?> AddUserAsync(CreateUserDto user, CancellationToken cancellationToken)
        {
            if (await _userRepository.GetSingleOrDefaultAsync(u=>u.Login==user.Login.Trim(),cancellationToken)!=null)
            {
                throw new BadRequestException("User login already exist");
            }
            //!!!  Get Role Admin from Db
            var userRole = await _userRoles.GetSingleOrDefaultAsync(r => r.Name == "Client",cancellationToken); 

            var userEntity = new ApplicationUser()
            {
                Login = user.Login.Trim(),
                PasswordHash = PasswordHashUtil.HashPassword(user.Password),
                // Add new user Role Client
                Roles = new []{new ApplicationUserApplicationRole(){ApplicatonUserRoleId = userRole!.Id}}
            };

            return _mapper.Map<GetUserDto>(await _userRepository.AddAsync(userEntity,cancellationToken));
        }

        public async Task<GetUserDto?> UpdateUserAsync(UpdateUserDto newUser, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetSingleOrDefaultAsync(u=>u.Id==newUser.Id, cancellationToken);
            if (user == null)
            {
                Log.Error($"User {newUser.Id} does not exist");
                throw new NotFoundException();
            }
            
            var currentLoggedInUserId = _currentUserSerice.CurrentUserId;
            var currentLoggedInUserRoles = _currentUserSerice.CurrentUserRoles;
            if (currentLoggedInUserRoles.Any(r => r != "Admin") && user.Id.ToString() != currentLoggedInUserId)
            {
                Log.Error($"User info {user.Login} cannot be updated by current User");
                throw new BadRequestException();
            }
            _mapper.Map(newUser, user);
            Log.Information($"User with id={newUser.Id} was updated");
            return _mapper.Map<GetUserDto>(await _userRepository.UpdateAsync(user, cancellationToken));
        } 
        public async Task<GetUserDto?> UpdateUserPasswordAsync(UpdateUserPasswordDto newUserPassword, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetSingleOrDefaultAsync(u=>u.Id== newUserPassword.Id, cancellationToken);
            if (user == null)
            {
                Log.Error($"User {newUserPassword.Id} does not exist");
                return null;
            }
            var currentLoggedInUserId = _currentUserSerice.CurrentUserId;
            var currentLoggedInUserRoles = _currentUserSerice.CurrentUserRoles;
            if (currentLoggedInUserRoles.Any(r => r != "Admin") && user.Id.ToString() != currentLoggedInUserId)
            {
                Log.Error($"User password {user.Login} cannot be updated by current User");
                throw new BadRequestException();
            }
            _mapper.Map(newUserPassword, user);
            user.PasswordHash = PasswordHashUtil.HashPassword(newUserPassword.PasswordHash);
            Log.Information($"User password with id={newUserPassword.Id} was updated");
            return _mapper.Map<GetUserDto>(await _userRepository.UpdateAsync(user, cancellationToken));
        }
       
        public async Task<int> CountAsync(string? nameFreeText, CancellationToken cancellationToken)
        {
            return await _userRepository.CountAsync(nameFreeText == null
                ? null
                : t => t.Login.Contains(nameFreeText, StringComparison.InvariantCultureIgnoreCase), cancellationToken);
        }
      
        public async Task<bool> RemoveUserAsync(int id, CancellationToken cancellationToken)
        {
            var userRemove = await _userRepository.GetSingleOrDefaultAsync(u => u.Id == id, cancellationToken);
            if (userRemove == null)
            {
                Log.Error($"User with id={id} does not exist");
                return false;
            }
            var currentLoggedInUserId = _currentUserSerice.CurrentUserId;
            var currentLoggedInUserRoles = _currentUserSerice.CurrentUserRoles;
            if (currentLoggedInUserRoles.Any(r => r != "Admin") && userRemove.Id.ToString() != currentLoggedInUserId)
            {
                Log.Error($"User {userRemove.Login} cannot be deleted by current User");
                throw new BadRequestException();
            }
            Log.Information($"User with id={id} was deleted");
            return await _userRepository.DeleteAsync(userRemove, cancellationToken);
        }
    }
}
