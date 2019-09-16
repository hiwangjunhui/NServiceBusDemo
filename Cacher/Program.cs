using NServiceBus;
using System;
using System.Threading.Tasks;

namespace Cacher
{
    class Program
    {
        static void Main(string[] args)
        {
            MainAsync().GetAwaiter().GetResult();
        }

        static async Task MainAsync()
        {
            var endpointConfiguration = Configuration.EndpointConfig.GetEndpointConfiguration();

            var endpointInstance = await Endpoint.Start(endpointConfiguration);

            Console.ReadLine();
            await endpointInstance.Stop();
        }
    }
}
