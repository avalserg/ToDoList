using System.Reflection;
using Common.Domain;
using Common.Repositories;
using Common.Repositories.Context;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Todos.Service.Mapping;

namespace Todos.Service.DI
{
    public static class TodosServicesInitializer
    {
        public static void InitializeRepositories(this IServiceCollection services)
        {
            services.AddTransient<IBaseRepository<Common.Domain.Todos>, BaseRepository<Common.Domain.Todos>>();
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
