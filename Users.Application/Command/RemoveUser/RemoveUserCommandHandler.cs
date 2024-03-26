using AutoMapper;
using Common.Application.Abstractions;
using Common.Application.Abstractions.Persistence;
using Common.Application.Exceptions;
using Common.Domain;
using MediatR;
using Serilog;

namespace Users.Application.Command.RemoveUser
{
    public class RemoveUserCommandHandler:IRequestHandler<RemoveUserCommand,bool>
    {
        private readonly IBaseRepository<ApplicationUser> _userRepository;
        private readonly ICurrentUserService _currentUserService;
        private readonly IBaseRepository<ApplicationUserRole> _userRoles;
        private readonly IMapper _mapper;
        private readonly UsersMemoryCache _usersMemoryCache;

        public RemoveUserCommandHandler(IBaseRepository<ApplicationUser> userRepository,ICurrentUserService currentUserService, IBaseRepository<ApplicationUserRole> userRoles,IMapper mapper, UsersMemoryCache usersMemoryCache)
        {
            _userRepository = userRepository;
            _currentUserService = currentUserService;
            _userRoles = userRoles;
            _mapper = mapper;
            _usersMemoryCache = usersMemoryCache;
        }
       
        public async Task<bool> Handle(RemoveUserCommand request, CancellationToken cancellationToken)
        {
            var userRemove = await _userRepository.GetSingleOrDefaultAsync(u => u.Id == request.Id, cancellationToken);
            if (userRemove == null)
            {
                Log.Error($"User with id={request.Id} does not exist");
                throw new NotFoundException();
            }
            var currentLoggedInUserId = _currentUserService.CurrentUserId;
            var currentLoggedInUserRoles = _currentUserService.CurrentUserRoles;
            if (currentLoggedInUserRoles.Any(r => r != "Admin") && userRemove.Id.ToString() != currentLoggedInUserId)
            {
                Log.Error($"User {userRemove.Login} cannot be deleted by current User");
                throw new ForbiddenExceptions();
            }
            _usersMemoryCache.Cache.Clear();
            Log.Information($"User with id={request.Id} was deleted");
            return await _userRepository.DeleteAsync(userRemove, cancellationToken);
        }
    }
}
