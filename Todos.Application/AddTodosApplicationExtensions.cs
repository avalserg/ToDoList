using Common.Api.Service;
using Common.Application.Abstractions;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using Common.Application.Abstractions.Persistence;
using Common.Domain;
using Common.Persistence;
using Todos.Application.Mapping;

namespace Todos.Application
{
    public static class AddTodosApplicationExtensions
    {
        public static void InitializeRepositories(this IServiceCollection services)
        {
            services.AddTransient<IBaseRepository<Common.Domain.Todos>, BaseRepository<Common.Domain.Todos>>();
            services.AddTransient<IBaseRepository<ApplicationUser>, BaseRepository<ApplicationUser>>();
        }

        public static void InitializeServices(this IServiceCollection services)
        {
            
            services.AddTransient<ICurrentUserService, CurrentUserService>();
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
        public static void AddCacheService(this IServiceCollection services)
        {
            services.AddSingleton<TodosMemoryCache>();
        }
        public static void AddMediatRService(this IServiceCollection services)
        {
            services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));
        }
    }
}
