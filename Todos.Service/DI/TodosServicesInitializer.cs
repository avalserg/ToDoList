using System.Reflection;
using Authorization.Service;
using Common.Domain;
using Common.Repositories;
using Common.Repositories.Context;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Todos.Service.Mapping;
using Users.Service;

namespace Todos.Service.DI
{
    public static class TodosServicesInitializer
    {
        public static void InitializeRepositories(this IServiceCollection services)
        {
            services.AddTransient<IBaseRepository<ApplicationUser>, BaseRepository<ApplicationUser>>();
            services.AddTransient<IBaseRepository<ApplicationUserApplicationRole>, BaseRepository<ApplicationUserApplicationRole>>();
            services.AddTransient<IBaseRepository<ApplicationUserRole>, BaseRepository<ApplicationUserRole>>();
            services.AddTransient<IBaseRepository<Common.Domain.Todos>, BaseRepository<Common.Domain.Todos>>();
            services.AddTransient<IBaseRepository<ApplicationUser>, BaseRepository<ApplicationUser>>();
        }

        public static void InitializeServices(this IServiceCollection services)
        {
            services.AddTransient<ITodosService, TodosService>();
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
