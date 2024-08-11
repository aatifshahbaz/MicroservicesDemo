using Common.Settings;
using Confluent.Kafka;
using Microsoft.Extensions.Configuration;
using System.Text.Json;

namespace Common.Kafka
{
    public class ProducerService : IDisposable, IProducerService
    {
        private readonly IProducer<string?, string> _producer;

        public ProducerService(IConfiguration configuration)
        {
            var kafkaSettings = configuration.GetSection("KafkaSettings").Get<KafkaProducer>();
            var acks = kafkaSettings.Acks ?? "";
            acks = acks.ToUpper();

            var producerconfig = new ProducerConfig
            {
                BootstrapServers = kafkaSettings.BootstraServers,
                Acks = acks.Equals("ALL") ? Acks.All : acks.Equals("LEADER") ? Acks.Leader : Acks.None,
                ClientId = kafkaSettings.ClientId ?? "",
            };

            //Create Producer in ProduceAsync if you want to auto-dispose of it and it will also be thread-safe, but it adds performance panelty
            _producer = new ProducerBuilder<string?, string>(producerconfig).Build();
        }


        public async Task ProduceAsync<T>(string topic, int partition, T @event, string? key = null) where T : class
        {
            //using var producer = new ProducerBuilder<string, string>(producerconfig).Build();

            var topicPartition = new TopicPartition(topic, new Partition(partition));

            string json = JsonSerializer.Serialize<T>(@event);
            var message = new Message<string?, string> { Key = key, Value = json };

            await _producer.ProduceAsync(topicPartition, message);
        }

        public void Dispose()
        {
            _producer?.Dispose();
        }
    }
}
