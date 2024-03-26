using MediatR;

namespace Todos.Application.Command.RemoveTodo
{
    public class RemoveTodoCommand:IRequest<bool>
    {
        public int Id { get; set; }
    }
}
