using System;
using NServiceBus;

namespace Workflow.Messages.Events
{
    public class P2pBulkWorkflowStarted : IEvent
    {
        public Guid WorkflowId { get; set; }
    }
}
