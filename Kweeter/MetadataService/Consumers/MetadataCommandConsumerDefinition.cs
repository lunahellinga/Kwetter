using MassTransit;

namespace MetadataService.Consumers;

public class MetadataCommandConsumerDefinition :
    ConsumerDefinition<MetadataCommandConsumer>
{
    protected override void ConfigureConsumer(IReceiveEndpointConfigurator endpointConfigurator,
        IConsumerConfigurator<MetadataCommandConsumer> consumerConfigurator)
    {
        endpointConfigurator.UseMessageRetry(r => r.Intervals(500, 1000));
    }
}