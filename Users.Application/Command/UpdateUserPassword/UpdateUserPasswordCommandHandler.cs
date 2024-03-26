using AutoMapper;
using Common.Application.Abstractions;
using Common.Application.Abstractions.Persistence;
using Common.Application.Exceptions;
using Common.Application.Utils;
using Common.Domain;
using MediatR;
using Serilog;
using Users.Application.Dtos;

namespace Users.Application.Command.UpdateUserPassword
{
    public class UpdateUserPasswordCommandHandler : IRequestHandler<UpdateUserPasswordCommand, UpdateUserPasswordDto>
    {
        private readonly IBaseRepository<ApplicationUser> _userRepository;
        private readonly IBaseRepository<ApplicationUserRole> _userRoles;
        private readonly ICurrentUserService _currentUserService;
        private readonly UsersMemoryCache _usersMemoryCache;
        private readonly IMapper _mapper;

        public UpdateUserPasswordCommandHandler(
            IBaseRepository<ApplicationUser> userRepository,
            IBaseRepository<ApplicationUserRole> userRoles,
            ICurrentUserService currentUserService,
            UsersMemoryCache usersMemoryCache,
            IMapper mapper)
        {
            _userRepository = userRepository;
            _userRoles = userRoles;
            _currentUserService = currentUserService;
            _usersMemoryCache = usersMemoryCache;
            _mapper = mapper;
        }
        public async Task<UpdateUserPasswordDto> Handle(UpdateUserPasswordCommand request, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetSingleOrDefaultAsync(u => u.Id == request.Id, cancellationToken);
            if (user == null)
            {
                Log.Error($"User {request.Id} does not exist");
                throw new NotFoundException();
            }

            var currentLoggedInUserId = _currentUserService.CurrentUserId;
            var currentLoggedInUserRoles = _currentUserService.CurrentUserRoles;
            if (currentLoggedInUserRoles.Any(r => r != "Admin") && user.Id.ToString() != currentLoggedInUserId)
            {
                Log.Error($"User password {user.Login} cannot be updated by current User");
                throw new BadRequestException();
            }
            _mapper.Map(request, user);
            user.PasswordHash = PasswordHashUtil.HashPassword(request.PasswordHash);
            _usersMemoryCache.Cache.Clear();
            Log.Information($"User with id={request.Id} was updated");
            return _mapper.Map<UpdateUserPasswordDto>(await _userRepository.UpdateAsync(user, cancellationToken));
        }
    }
}
