using System.Xml;
using AutoMapper;
using Common.Domain;
using Users.Service.Dto;

namespace Users.Service.Mapping
{
    public class AutoMapperProfile:Profile
    {
       public AutoMapperProfile()
        {
            CreateMap<UpdateUserDto, User>();
            CreateMap<CreateUserDto, User>();
        }
    }
}
