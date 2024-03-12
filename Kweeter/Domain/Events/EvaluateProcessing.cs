using MassTransit;

namespace Domain.Events;

public class EvaluateProcessing : CorrelatedBy<Guid>
{
    public Guid CorrelationId { get; init; }
}