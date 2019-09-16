using NServiceBus;
using Poller.Messages.Models;
using System;
using System.Collections.Generic;

namespace Workflow.Messages.Events
{
    public class P2pReceived : IEvent
    {
        public Guid WorkflowId { get; set; }

        public IEnumerable<Customer> Customers { get; set; }
    }
}
