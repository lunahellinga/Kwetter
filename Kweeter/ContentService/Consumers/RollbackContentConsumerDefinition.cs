using MassTransit;

namespace ContentService.Consumers;

public class RollbackContentConsumerDefinition :
    ConsumerDefinition<RollbackContentConsumer>
{
    protected override void ConfigureConsumer(IReceiveEndpointConfigurator endpointConfigurator,
        IConsumerConfigurator<RollbackContentConsumer> consumerConfigurator)
    {
        endpointConfigurator.UseMessageRetry(r => r.Intervals(500, 1000));
    }
}