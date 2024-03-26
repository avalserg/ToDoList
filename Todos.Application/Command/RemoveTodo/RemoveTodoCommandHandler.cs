using Common.Application.Abstractions;
using Common.Application.Abstractions.Persistence;
using Common.Application.Exceptions;
using MediatR;
using Serilog;

namespace Todos.Application.Command.RemoveTodo
{
    public class RemoveTodoCommandHandler:IRequestHandler<RemoveTodoCommand,bool>
    {
        
        private readonly IBaseRepository<Common.Domain.Todos> _todosRepository;
        private readonly TodosMemoryCache _todosMemoryCache;
        private readonly ICurrentUserService _currentUserService;
       

        public RemoveTodoCommandHandler(
            IBaseRepository<Common.Domain.Todos> todosRepository,
            TodosMemoryCache todosMemoryCache,
            ICurrentUserService currentUserService)
        {
            
            _todosRepository = todosRepository;
            _todosMemoryCache = todosMemoryCache;
            _currentUserService = currentUserService;
            
        }
       
        public async Task<bool> Handle(RemoveTodoCommand request, CancellationToken cancellationToken)
        {
            var todoRemove = await _todosRepository.GetSingleOrDefaultAsync(u => u.Id == request.Id, cancellationToken);
            if (todoRemove == null)
            {
                Log.Error($"Todo with id={request.Id} does not exist");
                throw new NotFoundException();
            }
            var currentLoggedInUserId = _currentUserService.CurrentUserId;
            var currentLoggedInUserRoles = _currentUserService.CurrentUserRoles;
            if (currentLoggedInUserRoles.Any(r => r != "Admin") && todoRemove.OwnerId.ToString() != currentLoggedInUserId)
            {
                Log.Error($"Todo {todoRemove.Id} cannot be deleted by current User");
                throw new ForbiddenExceptions();
            }
            _todosMemoryCache.Cache.Clear();
            Log.Information($"Todo with id={request.Id} was deleted");
            return await _todosRepository.DeleteAsync(todoRemove, cancellationToken);
        }
    }
}
