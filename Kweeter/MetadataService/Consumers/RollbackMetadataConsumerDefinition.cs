using MassTransit;

namespace MetadataService.Consumers;

public class RollbackMetadataConsumerDefinition :
    ConsumerDefinition<RollbackMetadataConsumer>
{
    protected override void ConfigureConsumer(IReceiveEndpointConfigurator endpointConfigurator,
        IConsumerConfigurator<RollbackMetadataConsumer> consumerConfigurator)
    {
        endpointConfigurator.UseMessageRetry(r => r.Intervals(500, 1000));
    }
}