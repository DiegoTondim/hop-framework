namespace Hop.Framework.Core.Configuration
{
    public interface IConfigurationConnection
    {
        IConfigurationConnection Database(string name);
        IConfigurationConnection Endpoint(string ip, int port);
        IConfigurationConnection AddProperty(string key, string value);
        IConfigurationConnection InitCommand(string sql);
    }

    public interface IConfigurationBuilder
    {
        void Build();
    }
}
