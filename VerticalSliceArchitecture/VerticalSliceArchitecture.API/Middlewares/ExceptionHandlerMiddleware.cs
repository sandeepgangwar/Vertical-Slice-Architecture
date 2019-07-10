using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Net;
using System.Threading.Tasks;
using VerticalSliceArchitecture.Infrastructure.Exceptions;

namespace VerticalSliceArchitecture.API.Middlewares
{
    public class ExceptionHandlerMiddleware
    {
        private readonly RequestDelegate _next;

        public ExceptionHandlerMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext httpContext, IHostingEnvironment env)
        {
            try
            {
                await _next(httpContext);
            }
            catch (Exception exception)
            {
                await HandleExcpetionAsync(httpContext, exception);
                if (httpContext.Response.StatusCode == (int)HttpStatusCode.InternalServerError && env.IsDevelopment())
                {
                    throw;
                }
            }
        }

        private async Task HandleExcpetionAsync(HttpContext context, Exception exception)
        {
            var errorCode = string.Empty;
            var statusCode = HttpStatusCode.InternalServerError;

            switch (exception)
            {
                case UnauthorizedAccessException unauthorizedAccessEx:
                    statusCode = HttpStatusCode.Unauthorized;
                    break;

                case InfrastructureException infrastructureEx:
                    statusCode = HttpStatusCode.BadRequest;
                    errorCode = infrastructureEx.Code;
                    break;

                case ResourceNotFoundException resourceNotFoundEx:
                    statusCode = HttpStatusCode.NotFound;
                    errorCode = resourceNotFoundEx.Code;
                    break;
            }

            context.Response.StatusCode = (int)statusCode;

            if (!string.IsNullOrWhiteSpace(errorCode))
            {
                var response = new { code = errorCode, message = exception.Message };
                var payload = JsonConvert.SerializeObject(response);
                context.Response.ContentType = "application/json";
                await context.Response.WriteAsync(payload);
            }
        }
    }

    public static class ExceptionHandlercsExtensions
    {
        public static IApplicationBuilder UseExceptionHandlerMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<ExceptionHandlerMiddleware>();
        }
    }
}