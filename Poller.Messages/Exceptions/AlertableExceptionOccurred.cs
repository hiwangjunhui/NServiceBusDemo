using NServiceBus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Poller.Messages.Exceptions
{
    public class AlertableExceptionOccurred : Exception, IEvent
    {
        public AlertableExceptionOccurred(string errorMessage) : base(errorMessage)
        {

        }

        public int Code { get; set; }
    }
}
