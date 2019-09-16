using NServiceBus;
using System;

namespace Scheduler.Messages.Commands
{
    public class DoP2pRefresh : ICommand
    {
        public Guid WorkflowId { get; set; }
    }
}
