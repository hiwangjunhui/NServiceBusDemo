using NServiceBus;
using System;

namespace Workflow.Sagas
{
    class P2pBulkRefreshWorkflowState : ContainSagaData
    {
        public Guid WorkflowId { get; set; }    
    }
}
