using Common.Application.Abstractions.Persistence;
using MediatR;
using Microsoft.Extensions.Caching.Memory;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Todos.Application.Query.GetCountTodos
{
    public class GetCountTodosQueryHandler: IRequestHandler<GetCountTodosQuery, int>
    {
        private readonly IBaseRepository<Common.Domain.Todos> _todosRepository;
        private readonly MemoryCache _todosMemoryCache;


        public GetCountTodosQueryHandler(IBaseRepository<Common.Domain.Todos> todosRepository,TodosMemoryCache todosMemoryCache)
        {
            _todosRepository = todosRepository;
            _todosMemoryCache = todosMemoryCache.Cache;
        }


        public async Task<int> Handle(GetCountTodosQuery request, CancellationToken cancellationToken)
        {
            var cacheKey = JsonSerializer.Serialize($"Count:{request}", new JsonSerializerOptions() { ReferenceHandler = ReferenceHandler.IgnoreCycles });
            if (_todosMemoryCache.TryGetValue(cacheKey, out int? result))
                return result!.Value;
            if (string.IsNullOrEmpty(request.NameFreeText))
            {
                result = await _todosRepository.CountAsync(cancellationToken: cancellationToken);
                return result.Value;
            }

            result = await _todosRepository.CountAsync(u => u.Label.Contains(request.NameFreeText), cancellationToken);
            var cacheEntryOptions = new MemoryCacheEntryOptions()
                .SetAbsoluteExpiration(TimeSpan.FromSeconds(10))
                //.SetSlidingExpiration(TimeSpan.FromSeconds(3))
                .SetSize(1);
            _todosMemoryCache.Set(cacheKey, result, cacheEntryOptions);

            return result.Value;
        }
    }
}
