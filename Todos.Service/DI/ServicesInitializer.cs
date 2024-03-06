using System.Reflection;
using Common.Domain;
using Common.Repositories;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using Todos.Service.Mapping;

namespace Todos.Service.DI
{
    public static class ServicesInitializer
    {
        public static void InitializeRepositories(this IServiceCollection services)
        {
            services.AddTransient<IBaseRepository<Domain.Todos>, BaseRepository<Domain.Todos>>();
            services.AddTransient<IBaseRepository<User>, BaseRepository<User>>();
        }

        public static void InitializeServices(this IServiceCollection services)
        {
            services.AddTransient<ITodosService, TodosService>();
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
