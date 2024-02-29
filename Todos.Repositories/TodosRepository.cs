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

        public IReadOnlyCollection<Domain.Todos> GetAllToDo(int? offset, int? ownerId, string? labelFreeText)
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

            todos = todos.OrderBy(t => t.Id).ToList();

            if (offset.HasValue)
            {
                todos.Skip(offset.Value);
            }
            
            return (IReadOnlyCollection<Domain.Todos>)todos;
        }
        public Domain.Todos? GetToDoById(int id)
        {
            return Todos.SingleOrDefault(t => t.Id == id);
        }


       
        public Domain.Todos AddToDo(Domain.Todos toDo)
        {
            if (Todos.Count==0)
            {
                toDo.Id = 1;
            }
            else
            {
                toDo.Id = Todos.Max(todo => todo.Id) + 1;
            }
            
            Todos.Add(toDo);

            return toDo;
        }

      
        public Domain.Todos? UpdateToDo(int id, Domain.Todos newToDo)
        {
            var todo = Todos.SingleOrDefault(t => t.Id == id);

            todo.UpdateDate = newToDo.UpdateDate;
            todo.CreatedDate=newToDo.CreatedDate;
            todo.OwnerId = newToDo.OwnerId;
            todo.Label = newToDo.Label;
            todo.IsDone = newToDo.IsDone;
            todo.Label = newToDo.Label;
            
            return todo;
        }


        public bool RemoveToDo(int id)
        {
            var todo = Todos.SingleOrDefault(t => t.Id == id);

            if (todo != null)
            {
                Todos.Remove(todo);
                return true;
            }
            
            return false;
        }
    }
}
