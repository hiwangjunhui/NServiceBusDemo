using Cacher.Messages.Events;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NServiceBus.Testing;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Cacher.Tests.SadPath
{
    [TestClass]
    public class P2pReceivedIsHandled
    {
        [TestMethod]
        public async Task SendCachedEvent()
        {
            var handler = new Handlers.P2pReceivedHandler(string.Empty);
            var context = new TestableMessageHandlerContext();

            var workflowId = Guid.NewGuid();
            await handler.PublishP2pCached(context, null, workflowId);

            var publishedMessages = context.PublishedMessages;
            handler.ShouldSatisfyAllConditions(() => publishedMessages.Containing<P2pCached>().Any(t => t.Message.WorkflowId == workflowId && t.Message.InsertedItems == null).ShouldBe(true));
        }
    }
}
