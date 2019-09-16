using NServiceBus;
using System.Configuration;
using System.Reflection;

namespace Cacher.Configuration
{
    static class EndpointConfig
    {
        private static readonly string EndpointName;
        private static readonly string ErrorQueue;
        private static readonly string RabbitMqConnectionString;

        static EndpointConfig()
        {
            EndpointName = Assembly.GetExecutingAssembly().GetName().Name;
            ErrorQueue = ConfigurationManager.AppSettings[nameof(ErrorQueue)];
            RabbitMqConnectionString = ConfigurationManager.AppSettings[nameof(RabbitMqConnectionString)];
        }

        internal static EndpointConfiguration GetEndpointConfiguration()
        {
            var config = new EndpointConfiguration(EndpointName);
            config.UseSerialization<XmlSerializer>();
            config.UsePersistence<InMemoryPersistence>();
            config.SendFailedMessagesTo(ErrorQueue);
            config.Recoverability().Delayed(delayed => delayed.NumberOfRetries(0));
            config.EnableInstallers();

            var transport = config.UseTransport<RabbitMQTransport>();
            transport.UseConventionalRoutingTopology();
            transport.ConnectionString(RabbitMqConnectionString);

            return config;
        }
    }
}
