using AutoMapper;
using Common.Domain;
using Todos.Service.Dto;
using Users.Service.Dto;

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
