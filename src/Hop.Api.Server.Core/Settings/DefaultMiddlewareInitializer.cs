using Hop.Api.Server.Core.Middleware;
using Microsoft.AspNetCore.Builder;

namespace Hop.Api.Server.Core.Settings
{
    public static class DefaultMiddlewareInitializer
    {
        public static IApplicationBuilder UseHopErrorHandlerMiddleware(this IApplicationBuilder app)
        {
            app.UseMiddleware(typeof(ErrorHandlingMiddleware));
            return app;
        }
    }
}