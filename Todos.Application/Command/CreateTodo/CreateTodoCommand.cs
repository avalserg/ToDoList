using MediatR;
using Todos.Application.Dtos;

namespace Todos.Application.Command.CreateTodo
{
    public class CreateTodoCommand:IRequest<Common.Domain.Todos>
    {
        public string Label { get; set; } = default!;

    }
}
