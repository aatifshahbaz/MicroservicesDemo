namespace Common.Kafka
{
    public interface IConsumer<T>
    {
        string Topic { get; }

        int Partition { get; }

        Task ConsumeAsync(T? value);
    }

}
