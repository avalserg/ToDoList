using MediatR;
using Todos.Application.Dtos;

namespace Todos.Application.Command.UpdateTodo
{
    public class UpdateTodoCommand : IRequest<UpdateToDoDto>
    {
        public int Id { get; set; }
        public bool IsDone { get; set; }
        public string Label { get; set; } = default!;
        public int OwnerId { get; set; }
    }
}
