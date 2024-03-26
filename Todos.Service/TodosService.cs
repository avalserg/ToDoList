using AutoMapper;
using Common.Domain;
using Common.Repositories;
using Common.Service;
using Common.Service.Exceptions;
using Serilog;
using Todos.Service.Dto;


namespace Todos.Service
{
    public class TodosService : ITodosService
    {
       
        private readonly IBaseRepository<Common.Domain.Todos> _todosRepository;
        private readonly IBaseRepository<ApplicationUser> _userRepository;
        private readonly IMapper _mapper;
        private readonly ICurrentUserSerice _currentUserSerice;


        public TodosService(IBaseRepository<Common.Domain.Todos> todosRepository, IBaseRepository<ApplicationUser> userRepository,IMapper mapper, ICurrentUserSerice currentUserSerice)
        {
            _todosRepository = todosRepository;
            _userRepository = userRepository;
            _mapper = mapper;
            _currentUserSerice = currentUserSerice;
        }
         
        public async Task<IReadOnlyCollection<Common.Domain.Todos>> GetAllToDoAsync(int? offset, string? labelFreeText, int? limit, bool? descending,CancellationToken cancellationToken)
        {
            limit ??= 10;
            
            var currentLoggedInUserRoles = _currentUserSerice.CurrentUserRoles;
            if (currentLoggedInUserRoles.Any(r => r == "Admin"))
            {
                var listAdmin = await _todosRepository.GetListAsync(
                    offset,
                    limit,
                    labelFreeText == null
                        ? null : t => t.Label.Contains(labelFreeText),
                    t => t.Id,
                    descending,
                    cancellationToken);
                return listAdmin;
            }

            var currentLoggedInUserId = _currentUserSerice.CurrentUserId;

            var listUser = await _todosRepository.GetListAsync(
                offset,
                limit,
                labelFreeText == null
                    ? t=>t.OwnerId.ToString() == currentLoggedInUserId : t => t.Label.Contains(labelFreeText) && t.OwnerId.ToString() == currentLoggedInUserId,
                t => t.Id,
                descending,
                cancellationToken);
            return listUser;
          
        }

        public async Task<Common.Domain.Todos?> GetToDoByIdAsync(int id,CancellationToken cancellationToken)
        {
           
            var todo = await _todosRepository.GetSingleOrDefaultAsync(t => t.Id == id, cancellationToken);

            if (todo == null)
            {
                Log.Error($"Todo {todo.Id} does not exist");
                throw new NotFoundException();
            }

            var currentLoggedInUserId = _currentUserSerice.CurrentUserId;
            var currentLoggedInUserRoles = _currentUserSerice.CurrentUserRoles;

            if (currentLoggedInUserRoles.Any(r => r != "Admin")&&todo.OwnerId.ToString()!=currentLoggedInUserId)
            {
                Log.Error($"Access denied to Todo {todo.Id} by current user");
                throw new ForbiddenExceptions();
            }
            return todo;
        }

        public async Task<Common.Domain.Todos?> CreateToDoAsync(CreateTodoDto createToDo, CancellationToken cancellationToken = default)
        {
            var currentLoggedInUserId = _currentUserSerice.CurrentUserId;
     
            var todoEntity = _mapper.Map<CreateTodoDto,Common.Domain.Todos>(createToDo);
            
            todoEntity.CreatedDate = DateTime.UtcNow;
            // always false after created
            todoEntity.IsDone = false;
            todoEntity.OwnerId = int.Parse(currentLoggedInUserId);

            Log.Information($"Todo with id={createToDo} was created");
           
            return await _todosRepository.AddAsync(todoEntity,cancellationToken);
        }
       
        public async Task<Common.Domain.Todos?> UpdateToDoAsync(UpdateToDoDto updateToDo, CancellationToken cancellationToken = default)
        {
           
            var todoEntity =await _todosRepository.GetSingleOrDefaultAsync(t=>t.Id==updateToDo.Id, cancellationToken);

            if (todoEntity == null)
            {
                Log.Error($"Todo {updateToDo.Id} does not exist");
                throw new NotFoundException();
            }

            var ownerTodo = await _userRepository.GetSingleOrDefaultAsync(u => u.Id == todoEntity.OwnerId,cancellationToken);
            if (ownerTodo is null)
            {
                Log.Error($"User {todoEntity.OwnerId} does not exist");
                throw new ForbiddenExceptions();
            }

            var currentLoggedInUserId = _currentUserSerice.CurrentUserId;
            var currentLoggedInUserRoles = _currentUserSerice.CurrentUserRoles;

            if (currentLoggedInUserRoles.Any(r => r != "Admin") && todoEntity.OwnerId.ToString() != currentLoggedInUserId)
            {
                Log.Error($"Todos {updateToDo.Id} cannot be updated by User {ownerTodo.Login}");
                throw new BadRequestException();
            }
            _mapper.Map(updateToDo, todoEntity);
            
            todoEntity.UpdateDate = DateTime.UtcNow;

            Log.Information($"Todo with id={todoEntity.Id} was updated");

            return await _todosRepository.UpdateAsync(todoEntity, cancellationToken);
        }

        public async Task<int> CountAsync(string? labelFreeText, CancellationToken cancellationToken = default)
        {
            var currentLoggedInUserRoles = _currentUserSerice.CurrentUserRoles;
            var currentLoggedInUserId = _currentUserSerice.CurrentUserId;
            if (currentLoggedInUserRoles.Any(r => r == "Admin"))
            {
                return await _todosRepository.CountAsync(labelFreeText == null
                    ? null
                    : t => t.Label.Contains(labelFreeText), cancellationToken);
            }

           
            return await _todosRepository.CountAsync(labelFreeText == null
                ? t => t.OwnerId.ToString() == currentLoggedInUserId
                : t => t.OwnerId.ToString() == currentLoggedInUserId && t.Label.Contains(labelFreeText), cancellationToken);
        }

        public async Task<bool> RemoveToDoAsync(int id, CancellationToken cancellationToken = default)
        {
            
            var todoToRemove =await _todosRepository.GetSingleOrDefaultAsync(t => t.Id == id, cancellationToken);
            if (todoToRemove == null)
            {
                Log.Error($"Todo with id={id} does not exist");
                throw new NotFoundException();
            }
            var currentLoggedInUserId = _currentUserSerice.CurrentUserId;
            var currentLoggedInUserRoles = _currentUserSerice.CurrentUserRoles;
            if (currentLoggedInUserRoles.Any(r => r != "Admin") && todoToRemove.OwnerId.ToString() != currentLoggedInUserId)
            {
                Log.Error($"Todos {todoToRemove.Id} cannot be deleted by User");
                throw new ForbiddenExceptions();
            }
            Log.Information($"Todo with id={id} was deleted");
            
            return await _todosRepository.DeleteAsync(todoToRemove, cancellationToken);
        }

    }
}
