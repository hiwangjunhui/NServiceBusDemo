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

            var scheduler = new Scheduler();
            await scheduler.ScheduleEvery(endpointInstance);

            Console.ReadLine();
            await endpointInstance.Stop();
        }
    }
}
