using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NServiceBus;
using Workflow.Messages.Events;

namespace Poller.Handlers
{
    class P2pBulkWorkflowStartedHandler : IHandleMessages<P2pBulkWorkflowStarted>
    {
        public Task Handle(P2pBulkWorkflowStarted message, IMessageHandlerContext context)
        {
            throw new NotImplementedException();
        }
    }
}
