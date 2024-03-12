using MassTransit;

namespace DataService.Consumers;

public class TextCommandConsumerDefinition :
    ConsumerDefinition<TextCommandConsumer>
{
    protected override void ConfigureConsumer(IReceiveEndpointConfigurator endpointConfigurator,
        IConsumerConfigurator<TextCommandConsumer> consumerConfigurator)
    {
        endpointConfigurator.UseMessageRetry(r => r.Intervals(500, 1000));
    }
}