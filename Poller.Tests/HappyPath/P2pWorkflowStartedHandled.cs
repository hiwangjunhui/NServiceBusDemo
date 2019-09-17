using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shouldly;
using NServiceBus.Testing;
using Workflow.Messages.Events;
using Poller.Messages.Events;

namespace Poller.Tests.HappyPath
{
    [TestClass]
    public class P2pWorkflowStartedHandled
    {
        [TestMethod]
        public async Task HandleTest()
        {
            var handler = new Handlers.P2pWorkflowStartedHandler("Data Source=.;Initial Catalog=PdpDb;Integrated Security=True;");
            var context = new TestableMessageHandlerContext();
            var message = new P2pWorkflowStarted() { WorkflowId = Guid.NewGuid() };
            await handler.Handle(message, context);
            var publishedMessages = context.PublishedMessages;
            handler.ShouldSatisfyAllConditions(() =>
            {
                publishedMessages.Containing<P2pPolled>().Any(t => t.Message.WorkflowId == message.WorkflowId && t.Message.Customers != null).ShouldBe(true);
            });
        }
    }
}
