namespace Todos.Repositories
{
    public class TodosRepository : ITodosRepository
    {
        private static List<Domain.Todos> Todos = new()
        {
            new Domain.Todos()
            {
                Id = 1, OwnerId = 1,IsDone = true, Label = "Проверить почту", CreatedDate = DateTime.Now, UpdateDate = default
            },
            new Domain.Todos()
            {
                Id = 2,OwnerId = 2, IsDone = false, Label = "Сходить в магазин", CreatedDate = DateTime.Now, UpdateDate = default
            },
            new Domain.Todos()
            {
                Id = 3,OwnerId = 3, IsDone = false, Label = "Выгулять собаку", CreatedDate = DateTime.Now, UpdateDate = default
            },
            new Domain.Todos()
            {
                Id = 4,OwnerId = 1, IsDone = false, Label = "Сделать зарядку", CreatedDate = DateTime.Now, UpdateDate = default
            },
        };

        public IReadOnlyCollection<Domain.Todos> GetAllToDo(int? offset, int? ownerId, string? labelFreeText, int? limit = 10)
        {
            IEnumerable<Domain.Todos> todos = Todos;

            if (!string.IsNullOrWhiteSpace(labelFreeText))
            {
                todos = todos.Where(t => t.Label.Contains(labelFreeText, StringComparison.InvariantCultureIgnoreCase));
            }

            if (ownerId.HasValue)
            {
                todos = todos.Where(t => t.OwnerId.Equals(ownerId));
            }

            todos = todos.OrderBy(t => t.Id);

            if (offset.HasValue)
            {
                todos.Skip(offset.Value);
            }

            limit ??= 10;

            todos = todos.Take(limit.Value).ToList();

            return (IReadOnlyCollection<Domain.Todos>)todos;
        }
        public Domain.Todos? GetToDoById(int id)
        {
            return Todos.SingleOrDefault(t => t.Id == id);
        }


       
        public Domain.Todos AddToDo(Domain.Todos toDo, int ownerId)
        {
            toDo.Id = Todos.Max(todo => todo.Id) + 1;
            toDo.OwnerId = ownerId;
            Todos.Add(toDo);

            return toDo;
        }

      
        public Domain.Todos? UpdateToDo(int id, Domain.Todos newToDo)
        {
            var todo = Todos.SingleOrDefault(t => t.Id == id);

            todo.IsDone = newToDo.IsDone;
            todo.Label = newToDo.Label;
            
            return todo;
        }

        public Domain.Todos UpdateToDoIsDone(int id, bool isDone)
        {
            var todo = Todos.SingleOrDefault(t => t.Id == id);

            todo.IsDone = isDone;

            return todo;
        }

        public Domain.Todos RemoveToDo(int id)
        {
            var todo = Todos.SingleOrDefault(t => t.Id == id);

            Todos.Remove(todo);

            return todo;
        }
    }
}
