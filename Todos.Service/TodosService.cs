using AutoMapper;
using Common.Domain;
using Common.Repositories;
using Serilog;
using Todos.Service.Dto;

namespace Todos.Service
{
    public class TodosService : ITodosService
    {
       
        private readonly IBaseRepository<Domain.Todos> _todosRepository;
        private readonly IBaseRepository<User> _userRepository;
        private readonly IMapper _mapper;
     
        public TodosService(IBaseRepository<Domain.Todos> todosRepository, IBaseRepository<User> userRepository,IMapper mapper)
        {
            _todosRepository = todosRepository;
            _userRepository = userRepository;
            _mapper = mapper;
            if (_todosRepository.Count()>0)
            {
                return;
            }
           
            for (var i = 1; i < 4; i++)
            {
                _todosRepository.Add(new Domain.Todos { Id = i, OwnerId = i, Label = $"Todo {i}", IsDone = false, CreatedDate = DateTime.Now, UpdateDate = default });
                _userRepository.Add(new User { Id = i, Name = $"User {i}" });
            }
        }
       
        public IReadOnlyCollection<Domain.Todos> GetAllToDo(int? offset, string? labelFreeText, int? limit)
        {
            limit ??= 10;
            var list = _todosRepository.GetList(
                offset,
                limit,
                labelFreeText == null ? null : t => t.Label.Contains(labelFreeText,StringComparison.InvariantCultureIgnoreCase),
                t => t.Id);

            return list;
        }

        public Domain.Todos? GetToDoById(int id)
        {
            return _todosRepository.GetSingleOrDefault(t=>t.Id==id);
        }

        public Domain.Todos CreateToDo(CreateTodoDto createToDo)
        {
            var owner = _userRepository.GetSingleOrDefault(u=>u.Id==createToDo.OwnerId);
            if (owner is null)
            {
                Log.Error($"User {createToDo.OwnerId} does not exist");
                throw  new Exception($"User {createToDo.OwnerId} does not exist");
            }
            var todoEntity = _mapper.Map<CreateTodoDto,Domain.Todos>(createToDo);
            
            todoEntity.CreatedDate = DateTime.UtcNow;
            // always false after created
            todoEntity.IsDone = false;

            todoEntity.Id = _todosRepository.Count() == 0 ? 1 : _todosRepository.Count() + 1;

            Log.Information($"Todo with id={todoEntity.Id} was created");
           
            return _todosRepository.Add(todoEntity);
        }
        public Domain.Todos? UpdateToDo(UpdateToDoDto updateToDo)
        {

         
            var todoEntity = _todosRepository.GetSingleOrDefault(t=>t.Id==updateToDo.Id);
            if (todoEntity == null)
            {
                Log.Error($"Todo {updateToDo.Id} does not exist");
                return null;
            }

            var owner = _userRepository.GetSingleOrDefault(u => u.Id == todoEntity.OwnerId);
            if (owner is null)
            {
                Log.Error($"User {todoEntity.OwnerId} does not exist");
                throw new Exception("User does not exist");
            }

            _mapper.Map(updateToDo, todoEntity);
            
           
            todoEntity.UpdateDate = DateTime.UtcNow;

            Log.Information($"Todo with id={todoEntity.Id} was updated");

            return _todosRepository.Update(todoEntity);
        }

        public int Count(string? labelFreeText)
        {
            return _todosRepository.Count(labelFreeText == null
                ? null
                : t => t.Label.Contains(labelFreeText, StringComparison.InvariantCultureIgnoreCase));
        }

        public bool RemoveToDo(int id)
        {
            var todoToRemove = _todosRepository.GetSingleOrDefault(t => t.Id == id);
            if (todoToRemove == null)
            {
                Log.Error($"Todo with id={id} does not exist");
                return false;
            }

            Log.Information($"Todo with id={id} was deleted");

            return _todosRepository.Delete(todoToRemove);
        }

    }
}
