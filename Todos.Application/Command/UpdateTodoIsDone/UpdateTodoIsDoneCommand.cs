using MediatR;
using Todos.Application.Dtos;

namespace Todos.Application.Command.UpdateTodoIsDone
{
    public class UpdateTodoIsDoneCommand : IRequest<UpdateTodoIsDoneDto>
    {
        public int Id { get; set; }
        public bool IsDone { get; set; }
       public int OwnerId { get; set; }
    }
}
