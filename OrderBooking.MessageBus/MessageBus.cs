using Azure.Messaging.ServiceBus;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace OrderBooking.MessageBus
{
    public class MessageBus : IMessageBus
    {
        

        public async Task PublishMessage(object message, string topic_queue_name)
        {
            await using var client = new ServiceBusClient("A connection string");

            ServiceBusSender sender = client.CreateSender(topic_queue_name);

            var jsonMessage = JsonConvert.SerializeObject(message);
            ServiceBusMessage busmessage = new ServiceBusMessage(Encoding.UTF8.GetBytes(jsonMessage))
            {
                CorrelationId=Guid.NewGuid().ToString(),
            };

            await sender.SendMessageAsync(busmessage);
            await client.DisposeAsync();
        }
    }
}
