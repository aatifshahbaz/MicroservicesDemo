using Common.Settings;
using Confluent.Kafka;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using System.Text.Json;

namespace Common.Kafka
{
    public class ConsumerService<T> : BackgroundService
    {
        private readonly IConsumer<string?, string> _kafkaConsumer;
        private readonly IConsumer<T> _consumer;

        public ConsumerService(IConfiguration configuration, IConsumer<T> consumer)
        {
            _consumer = consumer;

            var kafkaSettings = configuration.GetSection("KafkaSettings").Get<KafkaConsumer>();
            var autoOffsetReset = kafkaSettings.AutoOffsetReset.ToUpper();

            var consumerConfig = new ConsumerConfig
            {
                BootstrapServers = kafkaSettings.BootstraServers,
                GroupId = kafkaSettings.GroupId,
                AutoOffsetReset = autoOffsetReset.Equals("EARLIEST") ? AutoOffsetReset.Earliest : autoOffsetReset.Equals("LATEST") ? AutoOffsetReset.Latest : AutoOffsetReset.Error,
                EnableAutoCommit = kafkaSettings.EnableAutoCommit.ToUpper().Equals("TRUE") ? true : false,
                EnableAutoOffsetStore = false, //Delaying Consumer to trigger autoOffsetCommit, If TRUE it will commit as soon as message read from partition 
                ClientId = kafkaSettings.ClientId ?? "",
            };

            _kafkaConsumer = new ConsumerBuilder<string?, string>(consumerConfig).Build();
        }

        protected override async Task ExecuteAsync(CancellationToken cancellationToken)
        {
            //It is used to return the control back to the calling thread hence unblocking it and execute the following code on different thread by placing
            //on task queue with same synchronization context
            await Task.Yield();

            //_kafkaConsumer.Subscribe(_consumer.Topic);  //Partition will be assigned by Kafka automaticaly, will not guarantee that expected partition to be assigned to right consumer

            _kafkaConsumer.Assign(new TopicPartition(_consumer.Topic, new Partition(_consumer.Partition))); //For explicit partition mapping to a consumer


            while (!cancellationToken.IsCancellationRequested)
            {

                var consumeResult = _kafkaConsumer.Consume(1000);

                if (consumeResult != null)
                {
                    var message = consumeResult.Message.Value;
                    var result = JsonSerializer.Deserialize<T>(message);

                    await _consumer.ConsumeAsync(result);

                    _kafkaConsumer.StoreOffset(consumeResult); //Storing offset commit in memory manually, afterwards it will be autoComitted if EnableAutoCommit is TRUE

                }

                await Task.Delay(TimeSpan.FromSeconds(10), cancellationToken);
            }

            _kafkaConsumer.Unassign();
            //_kafkaConsumer.Unsubscribe();  //Used when subscribe method is used.
        }


        public override void Dispose()
        {
            _kafkaConsumer.Close();
            _kafkaConsumer.Dispose();

            base.Dispose();
        }
    }
}
