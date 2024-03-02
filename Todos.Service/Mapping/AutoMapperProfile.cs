using AutoMapper;
using Todos.Service.Dto;

namespace Todos.Service.Mapping
{
    public class AutoMapperProfile:Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<UpdateToDoDto, Domain.Todos>();
            CreateMap<CreateTodoDto, Domain.Todos>();
        }
    }
}
