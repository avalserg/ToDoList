using Common.Repositories;
using Todos.Repositories;
using Todos.Service;

namespace Todos.Api
{
    public static class ServicesInitializer
    {
        public static void InitializeRepositories(this IServiceCollection services)
        {
            services.AddTransient<ITodosRepository, TodosRepository>();
            services.AddTransient<IUserRepository, UserRepository>();
        }

        public static void InitializeServices(this IServiceCollection services)
        {
            services.AddTransient<ITodosService, TodosService>();
            
        }
    }
}
