using MassTransit;

namespace Domain.Events;

public record RollbackMentions : CorrelatedBy<Guid>
{
    public Guid CorrelationId { get; init; }
}