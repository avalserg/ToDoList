using System.Text.Json;
using System.Text.Json.Serialization;
using AutoMapper;
using Common.Application.Abstractions.Persistence;
using Common.Domain;
using MediatR;
using Microsoft.Extensions.Caching.Memory;
using Users.Application.Dtos;

namespace Users.Application.Query.GetListUsers
{
    public class GetListUsersQueryHandler:IRequestHandler<GetListUsersQuery, IReadOnlyCollection<GetUserDto>>
    {
        private readonly IBaseRepository<ApplicationUser> _userRepository;
        private readonly IMapper _mapper;
        private readonly MemoryCache _memoryCache;

        public GetListUsersQueryHandler(IBaseRepository<ApplicationUser> userRepository, IMapper mapper, UsersMemoryCache usersMemoryCache)
        {
            _userRepository = userRepository;
            _mapper = mapper;
            _memoryCache = usersMemoryCache.Cache;
        }
       
        public async Task<IReadOnlyCollection<GetUserDto>> Handle(GetListUsersQuery request, CancellationToken cancellationToken)
        {
            request.Limit ??= 10;
            var cacheKey = JsonSerializer.Serialize(request, new JsonSerializerOptions() { ReferenceHandler = ReferenceHandler.IgnoreCycles });
            if (_memoryCache.TryGetValue(cacheKey, out IReadOnlyCollection<GetUserDto>? result))
                return result;
            result = _mapper.Map<IReadOnlyCollection<GetUserDto>>(await _userRepository.GetListAsync(
                request.Offset,
                request.Limit,
                request.NameFreeText == null ? null : u => u.Login.Contains(request.NameFreeText),
                u => u.Id,
                request.Descending,
                cancellationToken));
            var cacheEntryOptions = new MemoryCacheEntryOptions()
                .SetAbsoluteExpiration(TimeSpan.FromSeconds(10))
                .SetSlidingExpiration(TimeSpan.FromSeconds(3))
                .SetSize(3);
            _memoryCache.Set(cacheKey, result, cacheEntryOptions);

            return result;
        }
    }
}
