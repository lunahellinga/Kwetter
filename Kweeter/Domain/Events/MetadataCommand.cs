using MassTransit;

namespace Domain.Events;

public record MetadataCommand : CorrelatedBy<Guid>
{
    public Guid UserId { get; init; }
    public int ContentCount { get; init; }
    public int TagCount { get; init; }
    public int MentionCount { get; init; }
    public Guid CorrelationId { get; init; }
}