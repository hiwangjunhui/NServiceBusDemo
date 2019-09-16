using Cacher.Messages.Events;
using NServiceBus;
using NServiceBus.Logging;
using Poller.Messages.Events;
using Poller.Messages.Exceptions;
using Scheduler.Messages.Commands;
using System;
using System.Threading.Tasks;
using Workflow.Messages.Events;
using Workflow.Messages.Policies;

namespace Workflow.Sagas
{
    class P2pWorkflow : Saga<P2pRefreshWorkflowState>,
        IAmStartedByMessages<DoP2pRefresh>,
        IAmStartedByMessages<P2pPolled>,
        IHandleMessages<P2pPolled>,
        IHandleMessages<P2pCached>,
        IHandleTimeouts<P2pWorkflowWarningPolicy>
    {
        private readonly ILog _logger;

        public P2pWorkflow()
        {
            _logger = LogManager.GetLogger<P2pWorkflow>();
        }

        public async Task Handle(DoP2pRefresh message, IMessageHandlerContext context)
        {
            var warningPolicy = new P2pWorkflowWarningPolicy { WorkflowId = message.WorkflowId };
            await RequestTimeout(context, TimeSpan.FromMinutes(10), warningPolicy);
            var events = new P2pWorkflowStarted { WorkflowId = message.WorkflowId };
            await context.Publish(events).ConfigureAwait(false);

            _logger.Info($"{message.WorkflowId} started.");
        }

        public async Task Handle(P2pPolled message, IMessageHandlerContext context)
        {
            await context.Publish(new P2pReceived { WorkflowId = message.WorkflowId, Customers = message.Customers }).ConfigureAwait(false);
        }

        public async Task Handle(P2pCached message, IMessageHandlerContext context)
        {
            MarkAsComplete();
            await Task.Run(() => _logger.Info($"{message.WorkflowId} completed."));
        }

        public async Task Timeout(P2pWorkflowWarningPolicy state, IMessageHandlerContext context)
        {
            var error = new AlertableExceptionOccurred($"{state.WorkflowId} timeout.") { Code = -2 };
            await context.Publish(error);

            _logger.Warn($"{state.WorkflowId} timeout.");
        }

        protected override void ConfigureHowToFindSaga(SagaPropertyMapper<P2pRefreshWorkflowState> mapper)
        {
            mapper.ConfigureMapping<DoP2pRefresh>(t => t.WorkflowId).ToSaga(t => t.WorkflowId);
            mapper.ConfigureMapping<P2pPolled>(t => t.WorkflowId).ToSaga(t => t.WorkflowId);
            mapper.ConfigureMapping<P2pCached>(t => t.WorkflowId).ToSaga(t => t.WorkflowId);
        }
    }
}
