using MassTransit;

namespace Domain.Events;

public record TagCompleted : CorrelatedBy<Guid>
{
    public int TagCount { get; init; }
    public List<string> Tags { get; init; }
    public Guid CorrelationId { get; init; }
}