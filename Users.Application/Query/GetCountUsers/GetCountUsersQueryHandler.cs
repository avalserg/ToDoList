using System.Text.Json;
using System.Text.Json.Serialization;
using Common.Application.Abstractions.Persistence;
using Common.Domain;
using MediatR;
using Microsoft.Extensions.Caching.Memory;

namespace Users.Application.Query.GetCountUsers
{
    public class GetCountUsersQueryHandler: IRequestHandler<GetCountUsersQuery, int>
    {
        private readonly IBaseRepository<ApplicationUser> _userRepository;
        private readonly MemoryCache _usersMemoryCache;


        public GetCountUsersQueryHandler(IBaseRepository<ApplicationUser> userRepository,UsersMemoryCache usersMemoryCache)
        {
            _userRepository = userRepository;
            _usersMemoryCache = usersMemoryCache.Cache;
        }


        public async Task<int> Handle(GetCountUsersQuery request, CancellationToken cancellationToken)
        {
            var cacheKey = JsonSerializer.Serialize($"Count:{request}", new JsonSerializerOptions() { ReferenceHandler = ReferenceHandler.IgnoreCycles });
            if (_usersMemoryCache.TryGetValue(cacheKey, out int? result))
                return result!.Value;
            if (string.IsNullOrEmpty(request.NameFreeText))
            {
                result = await _userRepository.CountAsync(cancellationToken: cancellationToken);
                return result.Value;
            }

            result = await _userRepository.CountAsync(u => u.Login.Contains(request.NameFreeText), cancellationToken);
            var cacheEntryOptions = new MemoryCacheEntryOptions()
                .SetAbsoluteExpiration(TimeSpan.FromSeconds(10))
                .SetSlidingExpiration(TimeSpan.FromSeconds(3))
                .SetSize(1);
            _usersMemoryCache.Set(cacheKey, result, cacheEntryOptions);

            return result.Value;
        }
    }
}
