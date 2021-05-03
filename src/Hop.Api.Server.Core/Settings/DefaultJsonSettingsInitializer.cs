using Microsoft.AspNetCore.Builder;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Hop.Api.Server.Core.Settings
{
    public static class DefaultJsonSettingsInitializer
    {
        public static IApplicationBuilder UseHopDefaultJsonSettings(this IApplicationBuilder app)
        {
            JsonConvert.DefaultSettings = () => new JsonSerializerSettings
            {
                Formatting = Formatting.None,
                ContractResolver = new CamelCasePropertyNamesContractResolver(),
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            };

            return app;
        }
    }
}
