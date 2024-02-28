using Common.Domain;

namespace Common.Repositories
{
    public class UserRepository:IUserRepository
    {
        private static readonly List<User> Users = new()
        {
            new User()
            {
                Id = 1, Name = "User 1"
            },
            new User()
            {
                Id = 2, Name = "User 2"
            },
            new User()
            {
                Id = 3, Name = "User 3"
            },

        };
        public IReadOnlyCollection<User> GetAllUser(int? offset, string? labelFreeText, int? limit)
        {
            IEnumerable<User> users = Users;

            if (!string.IsNullOrWhiteSpace(labelFreeText))
            {
                users = users.Where(t => t.Name.Contains(labelFreeText, StringComparison.InvariantCultureIgnoreCase));
            }

            users = users.OrderBy(t => t.Id);

            if (offset.HasValue)
            {
                users.Skip(offset.Value);
            }

            limit ??= 10;

            users = users.Take(limit.Value).ToList();

            return (IReadOnlyCollection<User>)users;
        }

        public User? GetUserById(int id)
        {
            return Users.SingleOrDefault(t => t.Id == id);
        }

        public User AddUser(User toDo)
        {
            toDo.Id = Users.Max(todo => todo.Id) + 1;
            Users.Add(toDo);

            return toDo;
        }

        public User? UpdateUser(int id, User newUser)
        {
            var todo = Users.SingleOrDefault(t => t.Id == id);
            
            todo.Name = newUser.Name;

            return todo;
        }


        public User RemoveUser(int id)
        {
            var user = Users.SingleOrDefault(t => t.Id == id);

            Users.Remove(user);

            return user;
        }
    }
}
