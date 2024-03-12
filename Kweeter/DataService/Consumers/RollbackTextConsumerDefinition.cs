using MassTransit;

namespace DataService.Consumers;

public class RollbackTextConsumerDefinition :
    ConsumerDefinition<RollbackTextConsumer>
{
    protected override void ConfigureConsumer(IReceiveEndpointConfigurator endpointConfigurator,
        IConsumerConfigurator<RollbackTextConsumer> consumerConfigurator)
    {
        endpointConfigurator.UseMessageRetry(r => r.Intervals(500, 1000));
    }
}