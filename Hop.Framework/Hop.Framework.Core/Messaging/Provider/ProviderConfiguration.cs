namespace Hop.Framework.Core.Messaging.Provider
{
    public abstract class ProviderConfiguration : IProviderConfiguration
    {
        protected const string HostNameConst = "rabbit:Hostname";
        protected const string UserNameConst = "rabbit:Username";
        protected const string PasswordNameConst = "rabbit:Password";
        protected const string VirtualHostConst = "rabbit:VirtualHost";
        protected const string PersistentMessagesConst = "rabbit:PersistentMessages";
        protected const string PrefetchCountConst = "rabbit:PrefetchCount";

        //public string Host => ConfigurationManager.AppSettings[Host];
        //public string Password => ConfigurationManager.AppSettings[UserNameConst];
        //public string UserName => ConfigurationManager.AppSettings[PasswordNameConst];
        //public string VirtualHost => ConfigurationManager.AppSettings[VirtualHostConst];
        //public string PersistentMessages => ConfigurationManager.AppSettings[PersistentMessagesConst];
        //public string PrefetchCount => ConfigurationManager.AppSettings[PrefetchCountConst];
        public abstract string Host { get; set; }
        public abstract string Password { get; set; }
        public abstract string UserName { get; set; }
        public abstract string VirtualHost { get; set; }
        public abstract string PersistentMessages { get; set; }
        public abstract string PrefetchCount { get; set; }
    }
}
