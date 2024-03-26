using MediatR;

namespace Todos.Application.Query.GetTodoById
{
    public class GetTodoByIdQuery : IRequest<Common.Domain.Todos>
    {
        public int Id { get; set; }
    }
}
