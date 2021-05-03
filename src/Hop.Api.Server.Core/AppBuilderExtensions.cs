using Hop.Api.Server.Core.Middleware;
using Microsoft.AspNetCore.Builder;

namespace Hop.Api.Server.Core
{
    public static class AppBuilderExtensions
    {
        public static IApplicationBuilder UseHopErrorHandlerMiddleware(this IApplicationBuilder app)
        {
            app.UseMiddleware(typeof(ErrorHandlingMiddleware));
            return app;
        }
    }
}