using MassTransit;

namespace TagService.Consumers;

public class TagCommandConsumerDefinition :
    ConsumerDefinition<TagCommandConsumer>
{
    protected override void ConfigureConsumer(IReceiveEndpointConfigurator endpointConfigurator,
        IConsumerConfigurator<TagCommandConsumer> consumerConfigurator)
    {
        endpointConfigurator.UseMessageRetry(r => r.Intervals(500, 1000));
    }
}