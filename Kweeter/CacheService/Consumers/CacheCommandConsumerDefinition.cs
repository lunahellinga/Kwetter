using MassTransit;

namespace CacheService.Consumers;

public class CacheCommandConsumerDefinition :
    ConsumerDefinition<CacheCommandConsumer>
{
    protected override void ConfigureConsumer(IReceiveEndpointConfigurator endpointConfigurator,
        IConsumerConfigurator<CacheCommandConsumer> consumerConfigurator)
    {
        endpointConfigurator.UseMessageRetry(r => r.Intervals(500, 1000));
    }
}