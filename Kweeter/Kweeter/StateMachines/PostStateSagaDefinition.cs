using Domain.Events;
using Kweeter.StateMachines.States;
using MassTransit;

namespace Kweeter.StateMachines;

public class PostStateSagaDefinition :
    SagaDefinition<NewKweetState>
{
    private const int ConcurrencyLimit = 20; // this can go up, depending upon the database capacity

    public PostStateSagaDefinition()
    {
        // specify the message limit at the endpoint level, which influences
        // the endpoint prefetch count, if supported.
        Endpoint(e =>
        {
            e.Name = "saga-queue";
            e.PrefetchCount = ConcurrencyLimit;
        });
    }

    protected override void ConfigureSaga(IReceiveEndpointConfigurator endpointConfigurator,
        ISagaConfigurator<NewKweetState> sagaConfigurator)
    {
        endpointConfigurator.UseMessageRetry(r => r.Intervals(5, 1000));
        endpointConfigurator.UseInMemoryOutbox();

        var partition = endpointConfigurator.CreatePartitioner(1);
        sagaConfigurator.Message<CacheCompleted>(x => x.UsePartitioner(partition, m => m.Message.CorrelationId));
        sagaConfigurator.Message<ContentCompleted>(x => x.UsePartitioner(partition, m => m.Message.CorrelationId));
        sagaConfigurator.Message<MentionCompleted>(x => x.UsePartitioner(partition, m => m.Message.CorrelationId));
        sagaConfigurator.Message<MetadataCompleted>(x => x.UsePartitioner(partition, m => m.Message.CorrelationId));
        sagaConfigurator.Message<NewKweet>(x => x.UsePartitioner(partition, m => m.Message.CorrelationId));
        sagaConfigurator.Message<ProcessingRolledBack>(x => x.UsePartitioner(partition, m => m.Message.CorrelationId));
        sagaConfigurator.Message<TagCompleted>(x => x.UsePartitioner(partition, m => m.Message.CorrelationId));
        sagaConfigurator.Message<TextCompleted>(x => x.UsePartitioner(partition, m => m.Message.CorrelationId));
    }
}