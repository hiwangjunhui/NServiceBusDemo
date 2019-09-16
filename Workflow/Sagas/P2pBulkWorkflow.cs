using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NServiceBus;
using Scheduler.Messages.Commands;
using Workflow.Messages.Events;

namespace Workflow.Sagas
{
    class P2pBulkWorkflow : Saga<P2pBulkRefreshWorkflowState>, IAmStartedByMessages<DoP2pBulkRefresh>
    {
        public Task Handle(DoP2pBulkRefresh message, IMessageHandlerContext context)
        {
            var events = new P2pBulkWorkflowStarted { WorkflowId = message.WorkflowId };
            return context.Publish(events);
        }

        protected override void ConfigureHowToFindSaga(SagaPropertyMapper<P2pBulkRefreshWorkflowState> mapper)
        {
            mapper.ConfigureMapping<DoP2pBulkRefresh>(t => t.WorkflowId).ToSaga(t => t.WorkflowId);
        }
    }
}
