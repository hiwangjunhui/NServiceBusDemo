using Cacher.Messages.Events;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NServiceBus.Testing;
using Poller.Messages.Events;
using Scheduler.Messages.Commands;
using Shouldly;
using System;
using System.Linq;
using System.Threading.Tasks;
using Workflow.Messages.Events;
using Workflow.Sagas;

namespace Workflow.Tests.HappyPath
{
    [TestClass]
    public class P2pWorkflowHandled
    {
        private TestableMessageHandlerContext _context = new TestableMessageHandlerContext();
        private P2pWorkflow _handler = new P2pWorkflow();

        [TestMethod]
        public async Task HandleDoP2pRefreshTest()
        {
            var msg = new DoP2pRefresh() { WorkflowId = Guid.NewGuid() };
            await _handler.Handle(msg, _context).ConfigureAwait(false);

            var publishedMessages = _context.PublishedMessages;
            _handler.ShouldSatisfyAllConditions(() =>
            {
                publishedMessages.Containing<P2pWorkflowStarted>().Any(t => t.Message.WorkflowId == msg.WorkflowId).ShouldBe(true);
            });
        }

        [TestMethod]
        public async Task HandleP2pPolledTest()
        {
            var msg = new P2pPolled { WorkflowId = Guid.NewGuid()};
            await _handler.Handle(msg, _context).ConfigureAwait(false);
            var publishedMessages = _context.PublishedMessages;
            _handler.ShouldSatisfyAllConditions(() =>
            {
                publishedMessages.Containing<P2pReceived>().Any(t => t.Message.WorkflowId == msg.WorkflowId).ShouldBe(true);
            });
        }

        [TestMethod]
        public async Task HandleP2pCachedTest()
        {
            var msg = new P2pCached { WorkflowId = Guid.NewGuid() };
            await _handler.Handle(msg, _context).ConfigureAwait(false);
            Assert.AreEqual(true, _handler.Completed);
        }
    }
}
