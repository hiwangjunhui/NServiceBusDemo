using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NServiceBus;
using Poller.Messages.Models;
using Workflow.Messages.Events;
using Poller.Messages.Events;
using Poller.Messages.Exceptions;
using NServiceBus.Logging;

namespace Poller.Handlers
{
    class P2pWorkflowStartedHandler : IHandleMessages<P2pWorkflowStarted>
    {
        private static readonly string DbConnectionString;
        private readonly ILog _logger;

        static P2pWorkflowStartedHandler()
        {
            DbConnectionString = ConfigurationManager.ConnectionStrings[nameof(DbConnectionString)].ConnectionString;
        }

        public P2pWorkflowStartedHandler()
        {
            _logger = LogManager.GetLogger<P2pWorkflowStartedHandler>();
        }

        public async Task Handle(P2pWorkflowStarted message, IMessageHandlerContext context)
        {
            using (var db = new DbHelper.PdpDb(DbConnectionString))
            {
                try
                {
                    var list = await db.GetListAsync<Customer>("select * from T_Customer where Status=@Status", new { Status = 0 });
                    await context.Publish(new P2pPolled { WorkflowId = message.WorkflowId, Customers = list });
                }
                catch (Exception ex)
                {
                    await context.Publish(new AlertableExceptionOccurred(ex.Message) { Code = -1 });
                }
            }

            _logger.Info($"{message.WorkflowId} polled.");
        }
    }
}
