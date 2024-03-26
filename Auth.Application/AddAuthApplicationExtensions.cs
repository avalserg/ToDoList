using System.Reflection;
using Common.Application.Abstractions.Persistence;
using Common.Domain;
using Common.Persistence;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace Auth.Application
{
    public static class AddAuthApplicationExtensions
    {
        public static void InitializeAuthRepositories(this IServiceCollection services)
        {

            services.AddTransient<IBaseRepository<ApplicationUser>, BaseRepository<ApplicationUser>>();
            services.AddTransient<IBaseRepository<RefreshToken>, BaseRepository<RefreshToken>>();
        }
        
        public static void AddValidationAuthService(this IServiceCollection services)
        {
            services.AddValidatorsFromAssemblies(new[] { Assembly.GetExecutingAssembly() }, includeInternalTypes: true);
        }
        public static void AddMediatRService(this IServiceCollection services)
        {
            services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));
        }
    }
}
