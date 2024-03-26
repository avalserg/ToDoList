using MediatR;
using Todos.Application.Dtos;

namespace Todos.Application.Query.GetListTodos
{
    public class GetListTodosQuery : BaseTodosFilter, IRequest<IReadOnlyCollection<Common.Domain.Todos>>
    {
        public int? Offset { get; set; }
        public int? Limit { get; set; }
        public bool? Descending { get; set; }
    }
}
