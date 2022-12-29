using Microsoft.AspNetCore.Builder;

namespace AudioServer.Web.Middleware
{
    public static class MiddlewareExtensions
    {
        public static void UseExceptionHandlingMiddleware(this IApplicationBuilder app)
        {
            app.UseMiddleware<ExceptionHandlingMiddleware>();
        }
    }
}