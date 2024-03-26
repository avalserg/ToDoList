using AutoMapper;
using Todos.Application.Command.CreateTodo;
using Todos.Application.Command.UpdateTodo;
using Todos.Application.Dtos;

namespace Todos.Application.Mapping
{
    public class AutoMapperProfile:Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<UpdateToDoDto, Common.Domain.Todos>();
            CreateMap<Common.Domain.Todos, UpdateToDoDto>();
            CreateMap<CreateTodoDto, Common.Domain.Todos>();
            CreateMap<CreateTodoCommand, Common.Domain.Todos>();
            CreateMap<Common.Domain.Todos, CreateTodoCommand>();
            CreateMap<UpdateTodoCommand, Common.Domain.Todos>();
            CreateMap<Common.Domain.Todos, UpdateTodoCommand>();
            CreateMap<UpdateTodoIsDoneDto, Common.Domain.Todos>();
            CreateMap<Common.Domain.Todos, UpdateTodoIsDoneDto>();
        }
    }
}
