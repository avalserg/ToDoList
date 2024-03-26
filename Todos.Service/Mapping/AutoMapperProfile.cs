using AutoMapper;
using Todos.Service.Dto;

namespace Todos.Service.Mapping
{
    public class AutoMapperProfile:Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<UpdateToDoDto, Common.Domain.Todos>();
            CreateMap<CreateTodoDto, Common.Domain.Todos>();
            
        }
    }
}
