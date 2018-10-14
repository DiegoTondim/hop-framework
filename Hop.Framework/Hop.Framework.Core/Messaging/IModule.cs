namespace Hop.Framework.Core.Messaging
{
    public interface IIntegrationModule
    {
        string Name { get; }
        string DisplayName { get; }
        string Application { get; }
    }
}
