using NServiceBus;
using System;

namespace Workflow.Sagas
{
    public class P2pRefreshWorkflowState : ContainSagaData
    {
        public Guid WorkflowId { get; set; }
    }
}
