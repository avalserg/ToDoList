using Common.Repositories;
using Todos.Repositories;

namespace Todos.Service
{
    public class TodosService : ITodosService
    {
        private readonly ITodosRepository _todosRepository;
        private readonly IUserRepository _userRepository;

        public TodosService(ITodosRepository todosRepository, IUserRepository userRepository)
        {
            _todosRepository = todosRepository;
            _userRepository = userRepository;
        }

        
        public IReadOnlyCollection<Domain.Todos> GetAllToDo(int? offset, int? ownerId, string? labelFreeText, int? limit = 10)
        {
            limit ??= 10;
            
            return _todosRepository.GetAllToDo(offset, ownerId, labelFreeText).Take(limit.Value).ToList();
        }

        public Domain.Todos? GetToDoById(int id)
        {
            return _todosRepository.GetToDoById(id);
        }

        public Domain.Todos AddToDo(Domain.Todos toDo)
        {
            var owner = _userRepository.GetUserById(toDo.OwnerId);
            if (owner is null)
            {
                throw  new Exception("User does not exist");
            }
            toDo.CreatedDate = DateTime.UtcNow;
            return _todosRepository.AddToDo(toDo);
        }
        public Domain.Todos? UpdateToDo(int id, Domain.Todos newToDo)
        {
           
            var todo = _todosRepository.GetToDoById(id);
            if (todo==null)
            {
                return null;
            }
            var owner = _userRepository.GetUserById(todo.OwnerId);
            if (owner is null)
            {
                throw new Exception("User does not exist");
            }
            //we cannot change ownerId for todos after created
            newToDo.OwnerId=owner.Id;
            newToDo.UpdateDate = DateTime.UtcNow;
           
            return _todosRepository.UpdateToDo(id,newToDo);
        }
        
        public bool RemoveToDo(int id)
        {
            var todoToRemove = _todosRepository.GetToDoById(id);
            if (todoToRemove == null)
            {
                return false;
            }
            return _todosRepository.RemoveToDo(id);
        }
    }
}
