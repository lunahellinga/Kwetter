using MassTransit;

namespace MentionService.Consumers;

public class MentionCommandConsumerDefinition :
    ConsumerDefinition<MentionCommandConsumer>
{
    protected override void ConfigureConsumer(IReceiveEndpointConfigurator endpointConfigurator,
        IConsumerConfigurator<MentionCommandConsumer> consumerConfigurator)
    {
        endpointConfigurator.UseMessageRetry(r => r.Intervals(500, 1000));
        endpointConfigurator.UseInMemoryOutbox();
    }
}