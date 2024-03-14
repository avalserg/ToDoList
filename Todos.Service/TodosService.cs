using AutoMapper;
using Common.Domain;
using Common.Repositories;
using Common.Service.Exceptions;
using Serilog;
using Todos.Service.Dto;


namespace Todos.Service
{
    public class TodosService : ITodosService
    {
       
        private readonly IBaseRepository<Common.Domain.Todos> _todosRepository;
        private readonly IBaseRepository<User> _userRepository;
        private readonly IMapper _mapper;
    

        public TodosService(IBaseRepository<Common.Domain.Todos> todosRepository, IBaseRepository<User> userRepository,IMapper mapper)
        {
            _todosRepository = todosRepository;
            _userRepository = userRepository;
            _mapper = mapper;
            
            //if (_todosRepository.Count()>0)
            //{
            //    return;
            //}
           
            //for (var i = 1; i < 4; i++)
            //{
            //    _userRepository.Add(new User {  Login = $"User {i}" });
            //    _todosRepository.Add(new Common.Domain.Todos {OwnerId = i, Label = $"Todo {i}", IsDone = false, CreatedDate = DateTime.Now, UpdateDate = default });
            //}
        
        }
       
        public IReadOnlyCollection<Common.Domain.Todos> GetAllToDo(int? offset, string? labelFreeText, int? limit)
        {
            limit ??= 10;
            var list = _todosRepository.GetList(
                offset,
                limit,
                labelFreeText == null ? null : t => t.Label.Contains(labelFreeText),
                t => t.Id);

            return list;
        }    
        public async Task<IReadOnlyCollection<Common.Domain.Todos>> GetAllToDoAsync(int? offset, string? labelFreeText, int? limit)
        {
            limit ??= 10;
            var list = await _todosRepository.GetListAsync(
                offset,
                limit,
                labelFreeText == null ? null : t => t.Label.Contains(labelFreeText),
                t => t.Id);

            return list;
        }

        public  Common.Domain.Todos? GetToDoById(int id)
        {
            return  _todosRepository.GetSingleOrDefault(t=>t.Id==id);
        }
        public async Task<Common.Domain.Todos?> GetToDoByIdAsync(int id,CancellationToken cancellationToken)
        {
            return await _todosRepository.GetSingleOrDefaultAsync(t=>t.Id==id,cancellationToken);
        }

        public Common.Domain.Todos CreateToDo(CreateTodoDto createToDo)
        {
            var owner = _userRepository.GetSingleOrDefault(u=>u.Id==createToDo.OwnerId);
            if (owner is null)
            {
                Log.Error($"User {createToDo.OwnerId} does not exist");
                throw new BadRequestException();
            }
            var todoEntity = _mapper.Map<CreateTodoDto,Common.Domain.Todos>(createToDo);
            
            todoEntity.CreatedDate = DateTime.UtcNow;
            // always false after created
            todoEntity.IsDone = false;

            Log.Information($"Todo with id={createToDo} was created");
           
            return _todosRepository.Add(todoEntity);
        } 
        public async Task<Common.Domain.Todos?> CreateToDoAsync(CreateTodoDto createToDo)
        {
            var owner = await _userRepository.GetSingleOrDefaultAsync(u=>u.Id==createToDo.OwnerId);
            if (owner is null)
            {
                Log.Error($"User {createToDo.OwnerId} does not exist");
                throw new BadRequestException();
            }
            var todoEntity = _mapper.Map<CreateTodoDto,Common.Domain.Todos>(createToDo);
            
            todoEntity.CreatedDate = DateTime.UtcNow;
            // always false after created
            todoEntity.IsDone = false;

            Log.Information($"Todo with id={createToDo} was created");
           
            return await _todosRepository.AddAsync(todoEntity);
        }
        public Common.Domain.Todos? UpdateToDo(UpdateToDoDto updateToDo)
        {
            var todoEntity = _todosRepository.GetSingleOrDefault(t=>t.Id==updateToDo.Id);
            if (todoEntity == null)
            {
                Log.Error($"Todo {updateToDo.Id} does not exist");
                throw new NotFoundException();
            }

            var owner = _userRepository.GetSingleOrDefault(u => u.Id == todoEntity.OwnerId);
            if (owner is null)
            {
                Log.Error($"User {todoEntity.OwnerId} does not exist");
                throw new BadRequestException();
            }

            _mapper.Map(updateToDo, todoEntity);
            
           
            todoEntity.UpdateDate = DateTime.UtcNow;

            Log.Information($"Todo with id={todoEntity.Id} was updated");

            return _todosRepository.Update(todoEntity);
        }  
        public async Task<Common.Domain.Todos?> UpdateToDoAsync(UpdateToDoDto updateToDo)
        {
            var todoEntity =await _todosRepository.GetSingleOrDefaultAsync(t=>t.Id==updateToDo.Id);
            if (todoEntity == null)
            {
                Log.Error($"Todo {updateToDo.Id} does not exist");
                throw new NotFoundException();
            }

            var owner = await _userRepository.GetSingleOrDefaultAsync(u => u.Id == todoEntity.OwnerId);
            if (owner is null)
            {
                Log.Error($"User {todoEntity.OwnerId} does not exist");
                throw new BadRequestException();
            }

            _mapper.Map(updateToDo, todoEntity);
            
           
            todoEntity.UpdateDate = DateTime.UtcNow;

            Log.Information($"Todo with id={todoEntity.Id} was updated");

            return await _todosRepository.UpdateAsync(todoEntity);
        }

        public int Count(string? labelFreeText)
        {
            return _todosRepository.Count(labelFreeText == null
                ? null
                : t => t.Label.Contains(labelFreeText));
        } 
        public async Task<int> CountAsync(string? labelFreeText)
        {
            return await _todosRepository.CountAsync(labelFreeText == null
                ? null
                : t => t.Label.Contains(labelFreeText));
        }

        public bool RemoveToDo(int id)
        {
            var todoToRemove = _todosRepository.GetSingleOrDefault(t => t.Id == id);
            if (todoToRemove == null)
            {
                Log.Error($"Todo with id={id} does not exist");
                throw new NotFoundException();
            }
            
            Log.Information($"Todo with id={id} was deleted");
            
            return _todosRepository.Delete(todoToRemove);
        }
        public async Task<bool> RemoveToDoAsync(int id)
        {
            var todoToRemove =await _todosRepository.GetSingleOrDefaultAsync(t => t.Id == id);
            if (todoToRemove == null)
            {
                Log.Error($"Todo with id={id} does not exist");
                throw new NotFoundException();
            }
            
            Log.Information($"Todo with id={id} was deleted");
            
            return await _todosRepository.DeleteAsync(todoToRemove);
        }

    }
}
