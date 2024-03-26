using System.Reflection;
using Authorization.Service.Mapping;
using Common.Application.Abstractions.Persistence;
using Common.Domain;
using Common.Persistence;
using Common.Repositories;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;


namespace Authorization.Service.DI
{
    public static class AuthServicesInitializer
    {
        public static void InitializeRepositories(this IServiceCollection services)
        {

            services.AddTransient<IBaseRepository<ApplicationUser>, BaseRepository<ApplicationUser>>();
            services.AddTransient<IBaseRepository<ApplicationUserApplicationRole>, BaseRepository<ApplicationUserApplicationRole>>();
            services.AddTransient<IBaseRepository<ApplicationUserRole>, BaseRepository<ApplicationUserRole>>();
            services.AddTransient<IBaseRepository<RefreshToken>, BaseRepository<RefreshToken>>();
        }

        public static void InitializeServices(this IServiceCollection services)
        {
            services.AddTransient<IAuthUserService, AuthUserService>();
        }
        //public static void AddAutoMapperService(this IServiceCollection services)
        //{
        //    services.AddAutoMapper(typeof(AutoMapperProfile));
        //}
        public static void AddValidationService(this IServiceCollection services)
        {
            services.AddValidatorsFromAssemblies(new[] { Assembly.GetExecutingAssembly() }, includeInternalTypes: true);
        }
    }
}
