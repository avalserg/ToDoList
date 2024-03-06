using Common.Domain;
using Common.Repositories;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using Users.Service.Mapping;

namespace Users.Service.DI
{
    public static class ServicesInitializer
    {
        public static void InitializeRepositories(this IServiceCollection services)
        {

            services.AddTransient<IBaseRepository<User>, BaseRepository<User>>();
        }

        public static void InitializeServices(this IServiceCollection services)
        {
            services.AddTransient<IUserService, UserService>();
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
