using MassTransit;

namespace Domain.Events;

public record CacheCommand : CorrelatedBy<Guid>
{
    public Guid UserId { get; init; }
    public string UserName { get; init; }
    public string KweetText { get; init; }
    public List<string> Tags { get; init; }
    public List<string> MentionedUsers { get; init; }
    public List<string> ContentLinks { get; init; }
    public DateTime CreatedAt { get; init; }
    public Guid CorrelationId { get; init; }
}