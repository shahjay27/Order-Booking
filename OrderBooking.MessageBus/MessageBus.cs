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
        private string connectionString = "Endpoint=sb://orderbooking.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=dxy9vDi9FXRKFci/OHsS9hoaiYID1130b+ASbOMzLv4=";

        public async Task PublishMessage(object message, string topic_queue_name)
        {
            await using var client = new ServiceBusClient(connectionString);

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
