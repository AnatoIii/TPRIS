using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using AudioServer.Service.Exceptions;
using System;
using System.Net;
using System.Threading.Tasks;

namespace AudioServer.Web.Middleware
{
    public class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;

        public ExceptionHandlingMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next.Invoke(context);
            }
            catch (Exception ex)
            {
                await HandleExceptionMessageAsync(context, ex).ConfigureAwait(false);
            }
        }

        private static Task HandleExceptionMessageAsync(HttpContext context, Exception exception)
        {
            context.Response.ContentType = "application/json";
            int statusCode = exception switch
            {
                NotFoundException => (int)HttpStatusCode.NotFound,
                AlreadyExistsException => (int)HttpStatusCode.Conflict,
                InvalidFormatException => (int)HttpStatusCode.BadRequest,
                BadRequestException => (int)HttpStatusCode.BadRequest,
                UnauthorizedException => (int)HttpStatusCode.Unauthorized,
                _ => (int)HttpStatusCode.InternalServerError
            };
            var result = JsonConvert.SerializeObject(new
            {
                StatusCode = statusCode,
                ErrorMessage = exception.Message
            });
            context.Response.StatusCode = statusCode;
            return context.Response.WriteAsync(result);
        }

    }
}