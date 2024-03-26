using AutoMapper;
using Common.Domain;
using Users.Application.Command.UpdateUser;
using Users.Application.Command.UpdateUserPassword;
using Users.Application.Dtos;

namespace Users.Application.Mapping
{
    public class AutoMapperProfile:Profile
    {
       public AutoMapperProfile()
        {
            CreateMap<UpdateUserDto, ApplicationUser>();
            CreateMap<ApplicationUser, UpdateUserDto>();
            CreateMap<UpdateUserPasswordDto, ApplicationUser>();

            CreateMap<ApplicationUser, UpdateUserPasswordDto>();

            CreateMap<CreateUserDto, ApplicationUser>();
            CreateMap<ApplicationUser, GetUserDto>();
            CreateMap<UpdateUserCommand, ApplicationUser>();
            CreateMap<UpdateUserPasswordCommand, ApplicationUser>();
        }
    }
}
