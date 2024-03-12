using MassTransit;

namespace Domain.Events;

public record MentionCompleted : CorrelatedBy<Guid>
{
    public int MentionCount { get; init; }
    public List<string> MentionedUsers { get; init; }
    public Guid CorrelationId { get; init; }
}