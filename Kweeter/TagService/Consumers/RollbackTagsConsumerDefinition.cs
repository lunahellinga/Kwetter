using MassTransit;

namespace TagService.Consumers;

public class RollbackTagsConsumerDefinition :
    ConsumerDefinition<RollbackTagConsumer>
{
    protected override void ConfigureConsumer(IReceiveEndpointConfigurator endpointConfigurator,
        IConsumerConfigurator<RollbackTagConsumer> consumerConfigurator)
    {
        endpointConfigurator.UseMessageRetry(r => r.Intervals(500, 1000));
    }
}