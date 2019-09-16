using Cacher.Messages.Events;
using NServiceBus;
using NServiceBus.Logging;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Poller.Handlers
{
    class P2pCachedHandler : IHandleMessages<P2pCached>
    {
        private readonly ILog _logger;
        private readonly string _connecitonString;

        public P2pCachedHandler()
        {
            _logger = LogManager.GetLogger<P2pCachedHandler>();
            _connecitonString = ConfigurationManager.ConnectionStrings["DbConnectionString"].ConnectionString;
        }

        public async Task Handle(P2pCached message, IMessageHandlerContext context)
        {
            //update status.
            if (message.InsertedItems?.Any() ?? false)
            {
                using (var db = new DbHelper.PdpDb(_connecitonString))
                {
                    foreach (var item in message.InsertedItems)
                    {
                        var result = await db.UpdateStatusAsync($"Update T_Customer set [Status]='1' where ID=@ID", new { ID = item });
                        if (result > 0)
                        {
                            _logger.Info($"Update status of {item} successfully.");
                        }
                        else
                        {
                            _logger.Warn($"Update status of {item} failed.");
                        }
                    }
                }
            }
        }
    }
}
