
namespace Common.Kafka
{
    public interface IProducerService
    {
        Task ProduceAsync<T>(string topic, int partition, T @event, string? key = null) where T : class;
    }
}