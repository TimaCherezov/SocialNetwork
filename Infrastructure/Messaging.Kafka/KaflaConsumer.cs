using Application.Abstractions;
using Application.Settings; 
using Confluent.Kafka;
using Domain.Events;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;

namespace Messaging.Kafka;

public class KafkaConsumer<TMessage> : BackgroundService 
    where TMessage : IDomainEvent 
{
    private readonly IServiceProvider _serviceProvider; 
    private readonly KafkaSettings _settings;
    private IConsumer<string, TMessage> _consumer;

    public KafkaConsumer(
        IServiceProvider serviceProvider, 
        IOptions<KafkaSettings> kafkaSettings)
    {
        _serviceProvider = serviceProvider;
        _settings = kafkaSettings.Value;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var config = new ConsumerConfig
        {
            BootstrapServers = _settings.BootstrapServers,
            GroupId = _settings.GroupId,
            AutoOffsetReset = AutoOffsetReset.Earliest,
            EnableAutoCommit = false
        };

        _consumer = new ConsumerBuilder<string, TMessage>(config)
            .SetValueDeserializer(new KafkaJsonDeserializer<TMessage>())
            .Build();

        await ConsumeAsync(stoppingToken);
    }

    private async Task ConsumeAsync(CancellationToken cancellationToken)
    {
        _consumer.Subscribe(_settings.Topic);

        try
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                var result = _consumer.Consume(cancellationToken);

                using var scope = _serviceProvider.CreateScope();
                
                var dispatcher = scope.ServiceProvider.GetRequiredService<IEventDispatcher>();

                await dispatcher.DispatchAsync(result.Message.Value, cancellationToken);

                _consumer.Commit(result);
            }
        }
        catch (OperationCanceledException)
        {
            // Нормальное завершение
        }
        catch (Exception ex)
        {
            // Логирование
        }
        finally
        {
            _consumer.Close();
        }
    }

    public override void Dispose()
    {
        _consumer?.Dispose();
        base.Dispose();
    }
}