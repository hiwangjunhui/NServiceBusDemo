using NServiceBus;
using Scheduler.Messages.Commands;
using System;
using System.Threading.Tasks;
using Workflow.Messages.Events;

namespace Workflow.Sagas
{
    class P2pRefreshWorkflow : Saga<P2pRefreshWorkflowState>, IAmStartedByMessages<DoP2pRefresh>
    {
        public Task Handle(DoP2pRefresh message, IMessageHandlerContext context)
        {
            var events = new P2pWorkflowStarted { WorkflowId = message.WorkflowId };
            return context.Publish(events);
        }

        protected override void ConfigureHowToFindSaga(SagaPropertyMapper<P2pRefreshWorkflowState> mapper)
        {
            mapper.ConfigureMapping<DoP2pRefresh>(t => t.WorkflowId).ToSaga(t => t.WorkflowId);
        }
    }
}
