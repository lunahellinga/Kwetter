using MassTransit;

namespace ContentService.Consumers;

public class ContentCommandConsumerDefinition :
    ConsumerDefinition<ContentCommandConsumer>
{
    protected override void ConfigureConsumer(IReceiveEndpointConfigurator endpointConfigurator,
        IConsumerConfigurator<ContentCommandConsumer> consumerConfigurator)
    {
        endpointConfigurator.UseMessageRetry(r => r.Intervals(500, 1000));
    }
}