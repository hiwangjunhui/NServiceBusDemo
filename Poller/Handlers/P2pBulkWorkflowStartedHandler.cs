using NServiceBus;
using System.Threading.Tasks;
using Workflow.Messages.Events;

namespace Poller.Handlers
{
    class P2pBulkWorkflowStartedHandler : IHandleMessages<P2pBulkWorkflowStarted>
    {
        public Task Handle(P2pBulkWorkflowStarted message, IMessageHandlerContext context)
        {
            return Task.CompletedTask;
        }
    }
}
