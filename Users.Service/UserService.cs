using AutoMapper;
using Common.Domain;
using Common.Repositories;
using Common.Service.Exceptions;
using Microsoft.AspNetCore.Identity;
using Serilog;
using Users.Service.Dto;
using Users.Service.Dtos;
using Users.Service.Utils;

namespace Users.Service
{
    public class UserService:IUserService
    {
        private readonly IBaseRepository<ApplicationUser> _userRepository;
        private readonly IBaseRepository<ApplicationUserRole> _userRoles;
        private readonly IMapper _mapper;

        public UserService(IBaseRepository<ApplicationUser> userRepository,IBaseRepository<ApplicationUserRole> userRoles ,IMapper mapper)
        {
            _userRepository = userRepository;
            _userRoles = userRoles;
            _mapper = mapper;
        }

        public IReadOnlyCollection<ApplicationUser> GetAllUsers(int? offset, string? nameFreeText,  int? limit)
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
        public async Task<GetUserDto?> GetUserByLoginAsync(string login, CancellationToken cancellationToken)
        {
           
            return _mapper.Map<GetUserDto>( await _userRepository.GetSingleOrDefaultAsync(u=>u.Login==login, cancellationToken));
        }

        public ApplicationUser AddUser(CreateUserDto user)
        {
            var userEntity = _mapper.Map<CreateUserDto, ApplicationUser>(user);

            return _userRepository.Add(userEntity);
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

        public ApplicationUser? UpdateUser(UpdateUserDto newUser)
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
        public async Task<GetUserDto?> UpdateUserPasswordAsync(UpdateUserPasswordDto newUserPassword, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetSingleOrDefaultAsync(u=>u.Id== newUserPassword.Id, cancellationToken);
            if (user == null)
            {
                Log.Error($"User {newUserPassword.Id} does not exist");
                return null;
            }
            _mapper.Map(newUserPassword, user);
            user.PasswordHash = PasswordHashUtil.HashPassword(newUserPassword.PasswordHash);
            Log.Information($"User password with id={newUserPassword.Id} was updated");
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
