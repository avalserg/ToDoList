using AutoMapper;
using Common.Application.Abstractions.Persistence;
using Common.Application.Exceptions;
using Common.Domain;
using MediatR;
using Users.Application.Dtos;

namespace Users.Application.Query.GetUserById
{
    public class GetUserByIdQueryHandler:IRequestHandler<GetUserByIdQuery, GetUserDto>
    {
        private readonly IBaseRepository<ApplicationUser> _userRepository;
        private readonly IMapper _mapper;

        public GetUserByIdQueryHandler(IBaseRepository<ApplicationUser> userRepository, IMapper mapper)
        {
            _userRepository = userRepository;
            _mapper = mapper;
        }
        public async Task<GetUserDto> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetSingleOrDefaultAsync(u => u.Id == request.Id, cancellationToken);
            if (user == null)
            {
                throw new NotFoundException();
            }
            return _mapper.Map<GetUserDto>(user);
        }
    }
}
