using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NServiceBus;
using Workflow.Messages.Events;

namespace Poller.Handlers
{
    class P2pWorkflowStartedHandler : IHandleMessages<P2pWorkflowStarted>
    {
        private static readonly string DbConnectionString;

        static P2pWorkflowStartedHandler()
        {
            DbConnectionString = ConfigurationManager.ConnectionStrings[nameof(DbConnectionString)].ConnectionString;
        }

        public async Task Handle(P2pWorkflowStarted message, IMessageHandlerContext context)
        {
            using (var db = new DbHelper.PdpDb(DbConnectionString))
            {
                var list = await db.GetListAsync()
            }
        }
    }
}
