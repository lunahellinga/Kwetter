using MassTransit;

namespace Domain.Events;

public record CacheCompleted : CorrelatedBy<Guid>
{
    public Guid CorrelationId { get; init; }
}