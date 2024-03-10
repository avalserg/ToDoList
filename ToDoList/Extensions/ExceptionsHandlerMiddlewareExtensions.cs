using Todos.Api.CustomMiddlewares;

namespace Todos.Api.Extensions
{
    public static class ExceptionsHandlerMiddlewareExtensions
    {
        public static IApplicationBuilder UseExceptionsHandler(this IApplicationBuilder app)
        {
            app.UseMiddleware<ExceptionsHandlerMiddleware>();
            return app;
        }
    }
}
