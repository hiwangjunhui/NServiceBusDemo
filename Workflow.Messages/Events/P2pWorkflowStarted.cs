using NServiceBus;
using System;

namespace Workflow.Messages.Events
{
    public class P2pWorkflowStarted : IEvent
    {
        public Guid WorkflowId { get; set; }
    }
}
