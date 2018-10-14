namespace Hop.Framework.Core.Host
{
    public class HostConfiguration : IHostConfiguration
    {
        public string HostName { get; set; }

        public HostConfiguration(string hostName)
        {
            HostName = hostName;
        }
    }
}
