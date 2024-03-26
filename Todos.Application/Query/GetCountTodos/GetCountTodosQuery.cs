using MediatR;
using Todos.Application.Dtos;

namespace Todos.Application.Query.GetCountTodos
{
    public class GetCountTodosQuery:BaseTodosFilter,IRequest<int>
    {
    }
}
