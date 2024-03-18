using Common.Domain;
using Common.Repositories;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using Authorization.Service;
using Common.Service;
using Users.Service.Mapping;

namespace Users.Service.DI
{
    public static class UserServicesInitializer
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
            services.AddTransient<IUserService, UserService>();
            services.AddTransient<IAuthUserService, AuthUserService>();
            services.AddTransient<ICurrentUserSerice, CurrentUserService>();
            services.AddHttpContextAccessor();
        }
        public static void AddAutoMapperService(this IServiceCollection services)
        {
            services.AddAutoMapper(typeof(AutoMapperProfile));
        }
        public static void AddValidationService(this IServiceCollection services)
        {
            services.AddValidatorsFromAssemblies(new[] { Assembly.GetExecutingAssembly() }, includeInternalTypes: true);
        }
    }
}
