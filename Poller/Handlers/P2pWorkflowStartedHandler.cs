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
    public class P2pWorkflowStartedHandler : IHandleMessages<P2pWorkflowStarted>
    {
        private readonly string DbConnectionString;
        private readonly ILog _logger = LogManager.GetLogger<P2pWorkflowStartedHandler>();

        public P2pWorkflowStartedHandler()
        {
            DbConnectionString = ConfigurationManager.ConnectionStrings[nameof(DbConnectionString)]?.ConnectionString;
        }

        public P2pWorkflowStartedHandler(string connectionString)
        {
            DbConnectionString = connectionString;
        }

        public async Task Handle(P2pWorkflowStarted message, IMessageHandlerContext context)
        {
            using (var db = new DbHelper.PdpDb(DbConnectionString))
            {
                try
                {
                    var list = await db.GetListAsync<Customer>("select * from T_Customer where Status=@Status Order by Id OFFSET 0 Rows Fetch Next 10 Rows ONLY", new { Status = 0 });
                    await context.Publish(new P2pPolled { WorkflowId = message.WorkflowId, Customers = list }).ConfigureAwait(false);
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
