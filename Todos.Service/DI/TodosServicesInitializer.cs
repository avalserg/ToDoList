using Common.Api.Service;
using Common.Application.Abstractions.Persistence;
using Common.Domain;
using Common.Persistence;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using Todos.Service.Mapping;

namespace Todos.Service.DI
{
    public static class TodosServicesInitializer
    {
        public static void InitializeRepositories(this IServiceCollection services)
        {
            services.AddTransient<IBaseRepository<Common.Domain.Todos>, BaseRepository<Common.Domain.Todos>>();
            services.AddTransient<IBaseRepository<ApplicationUser>, BaseRepository<ApplicationUser>>();
        }

        public static void InitializeServices(this IServiceCollection services)
        {
            services.AddTransient<ITodosService, TodosService>();
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
