using AutoMapper;
using Common.Domain;
using Common.Repositories;
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
            //!!
            for (var i = 1; i < 4; i++)
            {
                _todosRepository.Add(new Domain.Todos { Id = i, OwnerId = i, Label = $"Todo {i}", IsDone = false, CreatedDate = DateTime.Now, UpdateDate = default});
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
                throw  new Exception("User does not exist");
            }
            var todoEntity = _mapper.Map<CreateTodoDto,Domain.Todos>(createToDo);
            
            todoEntity.CreatedDate = DateTime.UtcNow;
            // always false after created
            todoEntity.IsDone = false;

            todoEntity.Id = _todosRepository.Count() == 0 ? 1 : _todosRepository.Count() + 1;
            return _todosRepository.Add(todoEntity);
        }
        public Domain.Todos? UpdateToDo(UpdateToDoDto updateToDo)
        {

         
            var todoEntity = _todosRepository.GetSingleOrDefault(t=>t.Id==updateToDo.Id);
            if (todoEntity == null)
            {
                return null;
            }

            var owner = _userRepository.GetSingleOrDefault(u => u.Id == todoEntity.OwnerId);
            if (owner is null)
            {
                throw new Exception("User does not exist");
            }

            _mapper.Map(updateToDo, todoEntity);
            
            // we cannot change ownerId for todos after created
            todoEntity.OwnerId=owner.Id;
            todoEntity.UpdateDate = DateTime.UtcNow;
           
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
                return false;
            }
            return _todosRepository.Delete(todoToRemove);
        }
    }
}
