using NServiceBus;
using Scheduler.Messages.Commands;
using System;
using System.Threading.Tasks;

namespace Scheduler
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

            await endpointInstance.Send(new DoP2pBulkRefresh { WorkflowId = Guid.NewGuid() });
            await endpointInstance.Send(new DoP2pRefresh { WorkflowId = Guid.NewGuid() });

            Console.ReadLine();
            await endpointInstance.Stop();
        }
    }
}
