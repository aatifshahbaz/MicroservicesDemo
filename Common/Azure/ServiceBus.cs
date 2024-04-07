using Azure.Messaging.ServiceBus;
using Microsoft.Extensions.Configuration;
using System.Text.Json;

namespace Common.Azure
{
    //Haven't used this approach, instead used MassTransit over Azure Service Bus 
    public class ServiceBus
    {
        private readonly IConfiguration _config;

        public ServiceBus(IConfiguration config)
        {
            _config = config;
        }


        public async Task SendMessageAsync<T>(T message, string queueName)
        {
            // since ServiceBusClient implements IAsyncDisposable we create it with "await using"
            await using var client = new ServiceBusClient(_config.GetConnectionString("AzureServiceBus"));

            // create the sender
            ServiceBusSender sender = client.CreateSender(queueName);

            string messageBody = JsonSerializer.Serialize(message);

            // create a message that we can send. UTF-8 encoding is used when providing a string.
            ServiceBusMessage serviceBusMessage = new ServiceBusMessage(messageBody);

            // send the message
            await sender.SendMessageAsync(serviceBusMessage);
        }


        public async Task<T> ReceiveMessageAsync<T>(string queueName)
        {
            // since ServiceBusClient implements IAsyncDisposable we create it with "await using"
            await using var client = new ServiceBusClient(_config.GetConnectionString("AzureServiceBus"));

            // create a receiver that we can use to receive the message
            ServiceBusReceiver receiver = client.CreateReceiver(queueName);

            // the received message is a different type as it contains some service set properties
            ServiceBusReceivedMessage receivedMessage = await receiver.ReceiveMessageAsync();

            // get the message body as a string
            string messageBody = receivedMessage.Body.ToString();

            // Complete the message to remove it from the queue
            await receiver.CompleteMessageAsync(receivedMessage);

            return JsonSerializer.Deserialize<T>(messageBody);
        }
    }
}
