using MassTransit;

namespace Domain.Events;

public record ProcessingRolledBack : CorrelatedBy<Guid>
{
    public Guid CorrelationId { get; init; }
}