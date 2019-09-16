using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NServiceBus;
using Poller.Messages.Models;

namespace Poller.Messages.Events
{
    public class P2pPolled : IEvent
    {
        public Guid WorkflowId { get; set; }

        public IEnumerable<Customer> Customers { get; set; }
    }
}
