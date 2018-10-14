using Microsoft.Extensions.Configuration;
using System.IO;

namespace Hop.Framework.UnitTests.Configs
{
    public class ConfigurationBuilder
    {
        public static IConfigurationRoot Build(string jsonFile = "appsettings.json")
        {
            return new Microsoft.Extensions.Configuration.ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile(jsonFile)
                .Build();
        }
    }
}
