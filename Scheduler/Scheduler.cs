using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NServiceBus;
using NServiceBus.Logging;
using Scheduler.Properties;
using Commands = Scheduler.Messages.Commands;

namespace Scheduler
{
    sealed class Scheduler
    {
        private readonly ILog _logger;
        public Scheduler()
        {
            _logger = LogManager.GetLogger<Scheduler>();
        }

        public async Task ScheduleEvery(IEndpointInstance instance)
        {
            await instance.ScheduleEvery(TimeSpan.FromDays(Settings.Default.BulkScheduleIntervalInDays),
                async context =>
                {
                    var command = new Commands.DoP2pBulkRefresh { WorkflowId = Guid.NewGuid() };
                    _logger.Info($"A new {nameof(Commands.DoP2pBulkRefresh)} workflow [{command.WorkflowId}] started.");
                    await context.Send(command);
                }).ConfigureAwait(false);

            await instance.ScheduleEvery(TimeSpan.FromMinutes(Settings.Default.ScheduleIntervalInMinutes),
                async context =>
                {
                    var command = new Commands.DoP2pRefresh { WorkflowId = Guid.NewGuid() };
                    _logger.Info($"A new {nameof(Commands.DoP2pRefresh)} workflow [{command.WorkflowId}] started.");
                    await context.Send(command);
                }).ConfigureAwait(false);
        }
    }
}
