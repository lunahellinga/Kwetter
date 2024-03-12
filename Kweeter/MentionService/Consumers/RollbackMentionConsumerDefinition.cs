using MassTransit;

namespace MentionService.Consumers;

public class RollbackMentionConsumerDefinition :
    ConsumerDefinition<RollbackMentionsConsumer>
{
    protected override void ConfigureConsumer(IReceiveEndpointConfigurator endpointConfigurator,
        IConsumerConfigurator<RollbackMentionsConsumer> consumerConfigurator)
    {
        endpointConfigurator.UseMessageRetry(r => r.Intervals(500, 1000));
        endpointConfigurator.UseInMemoryOutbox();
    }
}