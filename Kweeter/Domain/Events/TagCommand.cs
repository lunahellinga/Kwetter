using MassTransit;

namespace Domain.Events;

public record TagCommand : CorrelatedBy<Guid>
{
    public Guid UserId { get; init; }
    public string KweetText { get; init; }
    public Guid CorrelationId { get; init; }
}