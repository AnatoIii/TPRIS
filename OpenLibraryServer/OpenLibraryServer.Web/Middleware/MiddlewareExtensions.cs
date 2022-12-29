using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;

namespace OpenLibraryServer.Web.Middleware
{
    public static class MiddlewareExtensions
    {
        public static void UseExceptionHandlingMiddleware(this IApplicationBuilder app)  
        {  
            app.UseMiddleware<ExceptionHandlingMiddleware>();  
        }
    }
}