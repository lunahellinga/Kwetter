using MassTransit;

namespace Domain.Events;

public record ContentCompleted : CorrelatedBy<Guid>
{
    public int ContentCount { get; set; }
    public List<string> ContentLinks { get; set; }
    public Guid CorrelationId { get; init; }
}