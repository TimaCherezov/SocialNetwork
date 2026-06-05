namespace Application.Abstractions;

public interface IKafkaProducer<in TMessage>
{
    Task ProduceAsync(TMessage message, CancellationToken cancellationToken = default);
}