using MassTransit;

namespace Domain.Events;

public record TextCompleted : CorrelatedBy<Guid>
{
    public string KweetText { get; init; }
    public Guid CorrelationId { get; init; }
}