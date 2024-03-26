using Microsoft.AspNetCore.Http;
using System.Net;
using System.Text.Json;
using Common.Application.Exceptions;
using FluentValidation;

namespace Common.Api
{
    public class ExceptionsHandlerMiddleware : IMiddleware
    {

        public async Task InvokeAsync(HttpContext httpContext, RequestDelegate next)
        {
            try
            {
                await next.Invoke(httpContext);
            }
            catch (Exception e)
            {
                var statusCode = HttpStatusCode.InternalServerError;
                var result = string.Empty;
                switch (e)
                {
                    case NotFoundException badRequestException:
                        statusCode = HttpStatusCode.NotFound;
                        result = JsonSerializer.Serialize(badRequestException.Message);
                        break;
                    case BadRequestException badRequestException:
                        statusCode = HttpStatusCode.BadRequest;
                        result = JsonSerializer.Serialize(badRequestException.Message);
                        break;
                    case ValidationException validationException:
                        statusCode = HttpStatusCode.BadRequest;
                        result = JsonSerializer.Serialize(validationException.Message);
                        break;
                    case ForbiddenExceptions forbiddenException:
                        statusCode = HttpStatusCode.Forbidden;
                        result = JsonSerializer.Serialize(forbiddenException.Message);
                        break;
                }

                if (string.IsNullOrWhiteSpace(result))
                {
                    result = JsonSerializer.Serialize(new { error = e.Message, innerMessage = e.InnerException?.Message, e.StackTrace });
                }

                httpContext.Response.StatusCode = (int)statusCode;
                httpContext.Response.ContentType = "application/json";
                await httpContext.Response.WriteAsync(result);
            }
        }


    }
}
