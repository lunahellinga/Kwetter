using MassTransit;

namespace Domain.Events;

public record RollbackMetadata : CorrelatedBy<Guid>
{
    public Guid CorrelationId { get; init; }
}