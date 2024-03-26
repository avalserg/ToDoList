using Common.Api.Service;
using Common.Application.Abstractions;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using Users.Application.Mapping;

namespace Users.Application
{
    public static class AddUserApplicationExtension
    {
       
        public static void InitializeUserServices(this IServiceCollection services)
        {
            services.AddTransient<ICurrentUserService, CurrentUserService>();
            services.AddHttpContextAccessor();
        }
        public static void AddAutoMapperUserService(this IServiceCollection services)
        {
            services.AddAutoMapper(typeof(AutoMapperProfile));
        }
        public static void AddValidationUserService(this IServiceCollection services)
        {
            services.AddValidatorsFromAssemblies(new[] { Assembly.GetExecutingAssembly() }, includeInternalTypes: true);
        }
       
        public static void AddCacheService(this IServiceCollection services)
        {
            services.AddSingleton<UsersMemoryCache>();
        } 
        public static void AddMediatRService(this IServiceCollection services)
        {
            services.AddMediatR(cfg=>cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));
        }
    }
}
