using MassTransit;

namespace Domain.Events;

public record MetadataCompleted : CorrelatedBy<Guid>
{
    public DateTime CreatedAt { get; set; }
    public Guid CorrelationId { get; init; }
}