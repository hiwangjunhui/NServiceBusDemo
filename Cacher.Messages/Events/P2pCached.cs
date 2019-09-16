using NServiceBus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cacher.Messages.Events
{
    public class P2pCached : IEvent
    {
        public Guid WorkflowId { get; set; }

        public List<Guid> InsertedItems { get; set; }
    }
}
