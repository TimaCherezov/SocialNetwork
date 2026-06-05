using Application.Abstractions;
using Application.Settings;
using Confluent.Kafka;
using Microsoft.Extensions.Options;

namespace Messaging.Kafka;

public class KafkaProducer<TMessage> : IKafkaProducer<TMessage>, IDisposable
{
    private readonly IProducer<string, TMessage> producer;
    private readonly string topic;

    public KafkaProducer(IOptions<KafkaSettings> kafkaSettings)
    {
        var config = new ProducerConfig
        {
            BootstrapServers = kafkaSettings.Value.BootstrapServers
        };

        producer = new ProducerBuilder<string, TMessage>(config)
            .SetValueSerializer(new KafkaJsonSerializer<TMessage>())
            .Build();

        topic = kafkaSettings.Value.Topic;
    }

    public async Task ProduceAsync(TMessage message, CancellationToken cancellationToken = default)
    {
        await producer.ProduceAsync(topic, new Message<string, TMessage>
        {
            Key = "uniq1",
            Value = message
        }, cancellationToken);
    }

    public void Dispose()
    {
        producer?.Dispose();
    }
}