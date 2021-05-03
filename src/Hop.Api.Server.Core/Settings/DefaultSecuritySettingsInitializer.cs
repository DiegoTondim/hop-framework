using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Cors.Internal;
using Microsoft.Extensions.DependencyInjection;

namespace Hop.Api.Server.Core.Settings
{
    public static class DefaultSecuritySettingsInitializer
    {
        public static IMvcBuilder AddMvcWithHopAuthFilter(this IServiceCollection services)
        {
            var builder = services.AddMvc();

            return builder;
        }

        private const string CorsPolicy = "CorsPolicy";

        public static IServiceCollection EnableHopCorsMode(this IServiceCollection services)
        {
            services.AddCors(options =>
            {
                options.AddPolicy(CorsPolicy,
                    corsBuilder =>
                    {
                        corsBuilder
                            .AllowAnyOrigin()
                            .AllowAnyMethod()
                            .AllowAnyHeader()
                            .AllowCredentials();
                    });
            });

            // Apply as default to all controllers. API etc
            services.Configure<MvcOptions>(options =>
            {
                options.Filters.Add(new CorsAuthorizationFilterFactory(CorsPolicy));
            });

            return services;
        }
    }
}