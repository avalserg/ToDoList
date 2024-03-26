using AutoMapper;
using Common.Application.Abstractions.Persistence;
using Common.Application.Exceptions;
using Common.Application.Utils;
using Common.Domain;
using MediatR;
using Users.Application.Dtos;

namespace Users.Application.Command.CreateUser
{
    public class CreateUserCommandHandler:IRequestHandler<CreateUserCommand,GetUserDto>
    {
        private readonly IBaseRepository<ApplicationUser> _userRepository;
        private readonly IBaseRepository<ApplicationUserRole> _userRoles;
        private readonly IMapper _mapper;
        private readonly UsersMemoryCache _usersMemoryCache;

        public CreateUserCommandHandler(IBaseRepository<ApplicationUser> userRepository, IBaseRepository<ApplicationUserRole> userRoles,IMapper mapper, UsersMemoryCache usersMemoryCache)
        {
            _userRepository = userRepository;
            _userRoles = userRoles;
            _mapper = mapper;
            _usersMemoryCache = usersMemoryCache;
        }
       
        public async Task<GetUserDto> Handle(CreateUserCommand request, CancellationToken cancellationToken)
        {
            if (await _userRepository.GetSingleOrDefaultAsync(u => u.Login == request.Login.Trim(), cancellationToken) != null)
            {
                throw new BadRequestException("User login already exist");
            }
            
            var userRole = await _userRoles.GetSingleOrDefaultAsync(r => r.Name == "Client", cancellationToken);

            var userEntity = new ApplicationUser()
            {
                Login = request.Login.Trim(),
                PasswordHash = PasswordHashUtil.HashPassword(request.Password),
                // Add new user Role Client
                Roles = new[] { new ApplicationUserApplicationRole() { ApplicatonUserRoleId = userRole!.Id } }
            };
            _usersMemoryCache.Cache.Clear();
            return _mapper.Map<GetUserDto>(await _userRepository.AddAsync(userEntity, cancellationToken));
        }
    }
}
