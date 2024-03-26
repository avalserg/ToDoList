using Common.Application.Abstractions.Persistence;
using Common.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Common.Persistence.Context
{
    public static class DbContextDi
    {
        public static IServiceCollection AddTodosDatabase(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<DbContext, ApplicationDbContext>(
                options =>
                {
                    options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"));
                }
            );
            services.AddTransient<IContextTransactionCreator, ContextTransactionCreator>();
            services.AddTransient<IBaseRepository<ApplicationUser>, BaseRepository<ApplicationUser>>();
            services.AddTransient<IBaseRepository<ApplicationUserRole>, BaseRepository<ApplicationUserRole>>();
            return services;
        }
    }
}
