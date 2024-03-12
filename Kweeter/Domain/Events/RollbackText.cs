using MassTransit;

namespace Domain.Events;

public record RollbackText : CorrelatedBy<Guid>
{
    public Guid CorrelationId { get; init; }
}