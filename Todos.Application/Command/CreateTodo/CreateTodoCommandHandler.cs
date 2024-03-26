using AutoMapper;
using Common.Application.Abstractions;
using Common.Application.Abstractions.Persistence;
using MediatR;
using Serilog;

namespace Todos.Application.Command.CreateTodo
{
    public class CreateTodoCommandHandler:IRequestHandler<CreateTodoCommand,Common.Domain.Todos>
    {
        
        private readonly IBaseRepository<Common.Domain.Todos> _todosRepository;
        private readonly ICurrentUserService _currentUserService;
        private readonly IMapper _mapper;
        private readonly TodosMemoryCache _todosMemoryCache;

        public CreateTodoCommandHandler(
            IBaseRepository<Common.Domain.Todos> todosRepository,
            ICurrentUserService currentUserService,
            IMapper mapper, 
            TodosMemoryCache todosMemoryCache)
        {
            
            _todosRepository = todosRepository;
            _currentUserService = currentUserService;
            
            _mapper = mapper;
            _todosMemoryCache = todosMemoryCache;
        }
       
        public async Task<Common.Domain.Todos> Handle(CreateTodoCommand request, CancellationToken cancellationToken)
        {
            var currentLoggedInUserId = _currentUserService.CurrentUserId;

            var todoEntity = _mapper.Map<Common.Domain.Todos>(request);
            
            todoEntity.CreatedDate = DateTime.UtcNow;
            // always false after created
            todoEntity.IsDone = false;
            todoEntity.OwnerId = int.Parse(currentLoggedInUserId);
            _todosMemoryCache.Cache.Clear();
            Log.Information($"Todo with id={request} was created");

            return await _todosRepository.AddAsync(todoEntity, cancellationToken);
        }
    }
}
