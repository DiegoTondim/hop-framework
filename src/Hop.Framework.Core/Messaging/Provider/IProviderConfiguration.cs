namespace Hop.Framework.Core.Messaging.Provider
{
    public interface IProviderConfiguration
    {
        string Host { get; set; }
        string UserName { get; set; }
        string Password { get; set; }
        string VirtualHost { get; set; }
        string PersistentMessages { get; set; }
        string PrefetchCount { get; set; }
    }
}
