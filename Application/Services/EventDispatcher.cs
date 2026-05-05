using Application.Abstractions;
using Domain.Events;
using Microsoft.Extensions.DependencyInjection;

namespace Application.Services;

public sealed class EventDispatcher(IServiceProvider serviceProvider) : IEventDispatcher
{
    public Task DispatchAsync<TEvent>(TEvent domainEvent, CancellationToken cancellationToken = default)
        where TEvent : IDomainEvent
    {
        var handlers = serviceProvider.GetServices<IEventHandler<TEvent>>();
        return DispatchCoreAsync(handlers, domainEvent, cancellationToken);
    }

    private static async Task DispatchCoreAsync<TEvent>(
        IEnumerable<IEventHandler<TEvent>> handlers,
        TEvent domainEvent,
        CancellationToken cancellationToken)
        where TEvent : IDomainEvent
    {
        foreach (var handler in handlers)
        {
            await handler.HandleAsync(domainEvent, cancellationToken);
        }
    }
}