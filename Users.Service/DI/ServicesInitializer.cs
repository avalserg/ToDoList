using Common.Domain;
using Common.Repositories;
using Microsoft.Extensions.DependencyInjection;
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

    }
}
