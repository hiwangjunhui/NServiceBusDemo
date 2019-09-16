using NServiceBus;
using System;

namespace Workflow.Sagas
{
    class P2pRefreshWorkflowState : ContainSagaData
    {
        public Guid WorkflowId { get; set; }
    }
}
