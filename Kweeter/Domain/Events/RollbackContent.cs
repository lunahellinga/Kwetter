using MassTransit;

namespace Domain.Events;

public record RollbackContent : CorrelatedBy<Guid>
{
    public Guid CorrelationId { get; init; }
}