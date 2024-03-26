using Common.Application.Abstractions;
using Common.Application.Abstractions.Persistence;
using MediatR;
using Microsoft.Extensions.Caching.Memory;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Todos.Application.Query.GetListTodos
{
    public class GetListTodosQueryHandler : IRequestHandler<GetListTodosQuery, IReadOnlyCollection<Common.Domain.Todos>>
    {
        private readonly ICurrentUserService _currentUserService;
        private readonly IBaseRepository<Common.Domain.Todos> _todosRepository;
        private readonly MemoryCache _memoryCache;

        public GetListTodosQueryHandler(
            ICurrentUserService currentUserService,
            IBaseRepository<Common.Domain.Todos> todosRepository,
            TodosMemoryCache todosMemoryCache
            )
        {
            _currentUserService = currentUserService;
            _todosRepository = todosRepository;
            _memoryCache = todosMemoryCache.Cache;
        }
        public async Task<IReadOnlyCollection<Common.Domain.Todos>> Handle(GetListTodosQuery request, CancellationToken cancellationToken)
        {
            request.Limit ??= 10;
            var cacheKey = JsonSerializer.Serialize(request, new JsonSerializerOptions() { ReferenceHandler = ReferenceHandler.IgnoreCycles });
            var cacheEntryOptions = new MemoryCacheEntryOptions()
                .SetAbsoluteExpiration(TimeSpan.FromSeconds(10))
                .SetSlidingExpiration(TimeSpan.FromSeconds(3))
                .SetSize(3);
            var currentLoggedInUserRoles = _currentUserService.CurrentUserRoles;
            if (currentLoggedInUserRoles.Any(r => r == "Admin"))
            {
                if (_memoryCache.TryGetValue(cacheKey, out IReadOnlyCollection<Common.Domain.Todos>? listAdmin))
                    return listAdmin;
                listAdmin = await _todosRepository.GetListAsync(
                    request.Offset,
                    request.Limit,
                    request.NameFreeText == null
                        ? null : t => t.Label.Contains(request.NameFreeText),
                    t => t.Id,
                    request.Descending,
                    cancellationToken);
                _memoryCache.Set(cacheKey, listAdmin, cacheEntryOptions);
                return listAdmin;
            }

            var currentLoggedInUserId = _currentUserService.CurrentUserId;
            if (_memoryCache.TryGetValue(cacheKey, out IReadOnlyCollection<Common.Domain.Todos>? listUser))
                return listUser;
            listUser = await _todosRepository.GetListAsync(
                request.Offset,
                request.Limit,
                request.NameFreeText == null
                    ? t => t.OwnerId.ToString() == currentLoggedInUserId : t => t.Label.Contains(request.NameFreeText) && t.OwnerId.ToString() == currentLoggedInUserId,
                t => t.Id,
                request.Descending,
                cancellationToken);
            _memoryCache.Set(cacheKey, listUser, cacheEntryOptions);
            return listUser;
        }
    }
}
