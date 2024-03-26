using Common.Application.Abstractions;
using Common.Application.Abstractions.Persistence;
using Common.Application.Exceptions;
using MediatR;
using Microsoft.Extensions.Caching.Memory;
using System.Text.Json.Serialization;
using System.Text.Json;

namespace Todos.Application.Query.GetTodoById
{
    public class GetTodoByIdQueryHandler : IRequestHandler<GetTodoByIdQuery, Common.Domain.Todos>
    {
        
        private readonly IBaseRepository<Common.Domain.Todos> _todosRepository;
        private readonly ICurrentUserService _currentUserService;
        private readonly MemoryCache _memoryCache;

        public GetTodoByIdQueryHandler(IBaseRepository<Common.Domain.Todos> todosRepository, ICurrentUserService currentUserService, TodosMemoryCache todosMemoryCache)
        {
            
            _todosRepository = todosRepository;
            _currentUserService = currentUserService;
            _memoryCache = todosMemoryCache.Cache;
        }
        public async Task<Common.Domain.Todos> Handle(GetTodoByIdQuery request, CancellationToken cancellationToken)
        {
            var cacheKey = JsonSerializer.Serialize(request, new JsonSerializerOptions() { ReferenceHandler = ReferenceHandler.IgnoreCycles });
            
            if (_memoryCache.TryGetValue(cacheKey, out Common.Domain.Todos? todo))
                return todo;
            todo = await _todosRepository.GetSingleOrDefaultAsync(t => t.Id == request.Id, cancellationToken);

            if (todo == null)
            {
                throw new NotFoundException();
            }
          
            var currentLoggedInUserId = _currentUserService.CurrentUserId;
            var currentLoggedInUserRoles = _currentUserService.CurrentUserRoles;

            if (currentLoggedInUserRoles.Any(r => r != "Admin") && todo.OwnerId.ToString() != currentLoggedInUserId)
            {
                throw new ForbiddenExceptions();
            }
            var cacheEntryOptions = new MemoryCacheEntryOptions()
                .SetAbsoluteExpiration(TimeSpan.FromSeconds(10))
                .SetSlidingExpiration(TimeSpan.FromSeconds(3))
                .SetSize(3);
            _memoryCache.Set(cacheKey, todo, cacheEntryOptions);
            return todo;
        }
    }
}
