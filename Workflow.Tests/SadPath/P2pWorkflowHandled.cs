using Microsoft.VisualStudio.TestTools.UnitTesting;
using NServiceBus.Testing;
using Poller.Messages.Exceptions;
using Shouldly;
using System;
using System.Linq;
using System.Threading.Tasks;
using Workflow.Messages.Policies;
using Workflow.Sagas;

namespace Workflow.Tests.SadPath
{
    [TestClass]
    public class P2pWorkflowHandled
    {
        private TestableMessageHandlerContext _context = new TestableMessageHandlerContext();
        private P2pWorkflow _handler = new P2pWorkflow();

        [TestMethod]
        public async Task TimeoutTest()
        {
            var state = new P2pWorkflowWarningPolicy { WorkflowId = Guid.NewGuid()};
            await _handler.Timeout(state, _context).ConfigureAwait(false);

            var publishedMessages = _context.PublishedMessages;
            _handler.ShouldSatisfyAllConditions(() =>
            {
                publishedMessages.Containing<AlertableExceptionOccurred>().Any(t => t.Message.Code == -2 && t.Message.Message == $"{state.WorkflowId} timeout.").ShouldBe(true);
            });
        }
    }
}
