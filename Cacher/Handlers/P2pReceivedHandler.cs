using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cacher.Messages.Events;
using NServiceBus;
using NServiceBus.Logging;
using Workflow.Messages.Events;

namespace Cacher.Handlers
{
    public class P2pReceivedHandler : IHandleMessages<P2pReceived>
    {
        private readonly string _connecitonString;
        private readonly ILog _logger = LogManager.GetLogger<P2pReceivedHandler>();

        public P2pReceivedHandler()
        {
            _connecitonString = ConfigurationManager.ConnectionStrings["DbConnectionString"]?.ConnectionString;
        }

        public P2pReceivedHandler(string connectionString)
        {
            _connecitonString = connectionString;
        }

        public async Task Handle(P2pReceived message, IMessageHandlerContext context)
        {
            var insertedItems = new List<Guid>();

            if (message.Customers?.Any() ?? false)
            {
                using (var db = new DbHelper.PdpDb(_connecitonString))
                {
                    foreach (var item in message.Customers)
                    {
                        var result = await db.Insert(item);
                        if (result > 0)
                        {
                            insertedItems.Add(item.Id);//added id to list that inserted successfully.
                            _logger.Info($"{item} inserted successfully.");
                        }
                    }
                }
            }

            //if no data polled, publish event also.
            await PublishP2pCached(context, insertedItems, message.WorkflowId);

            _logger.Info($"{message.WorkflowId} cached.");
        }

        public async Task PublishP2pCached(IMessageHandlerContext context, List<Guid> insertedItems, Guid workflowId)
        {
            var message = new P2pCached { WorkflowId = workflowId, InsertedItems = insertedItems };
            await context.Publish(message);
        }
    }
}
