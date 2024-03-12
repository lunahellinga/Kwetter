using MassTransit;

namespace Domain.Events;

public record RollbackTags : CorrelatedBy<Guid>
{
    public Guid CorrelationId { get; init; }
}