using NServiceBus;
using System;

namespace Scheduler.Messages.Commands
{
    public class DoP2pBulkRefresh : ICommand
    {
        public Guid WorkflowId { get; set; }
    }
}
