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
           return _todosRepository.GetAllToDo(offset, ownerId, labelFreeText, limit);
        }

        public Domain.Todos? GetToDoById(int id)
        {
            return _todosRepository.GetToDoById(id);
        }

        public Domain.Todos AddToDo(Domain.Todos toDo, int ownerId)
        {
            var owner = _userRepository.GetUserById(ownerId);
            if (owner is null)
            {
                throw  new Exception("User does not exist");
            }
            toDo.CreatedDate = DateTime.UtcNow;
            return _todosRepository.AddToDo(toDo, ownerId);
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
            todo.OwnerId=owner.Id;
            todo.UpdateDate = DateTime.UtcNow;
            return _todosRepository.UpdateToDo(id,newToDo);
        }
        public Domain.Todos UpdateToDoIsDone(int id, bool isDone)
        {
           return _todosRepository.UpdateToDoIsDone(id, isDone);
        }
        public Domain.Todos RemoveToDo(int id)
        {
            var todoToRemove = _todosRepository.GetToDoById(id);
            if (todoToRemove == null)
            {
                return null;
            }
            return _todosRepository.RemoveToDo(id);
        }
    }
}
