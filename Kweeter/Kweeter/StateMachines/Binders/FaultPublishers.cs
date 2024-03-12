using Domain.Events;
using Kweeter.StateMachines.States;
using MassTransit;

namespace Kweeter.StateMachines.Binders;

public static class FaultPublishers
{
    public static EventActivityBinder<NewKweetState, Fault<CacheCommand>> PublishRevertMetadata(
        this EventActivityBinder<NewKweetState, Fault<CacheCommand>> binder)
    {
        return binder.PublishAsync(context =>
            context.Init<Fault<MetadataCompleted>>(
                new { context.Message.Message.CorrelationId }
            )
        );
    }

    public static EventActivityBinder<NewKweetState, Fault<MetadataCommand>> PublishRevertProcessing(
        this EventActivityBinder<NewKweetState, Fault<MetadataCommand>> binder)
    {
        return binder.PublishAsync(context =>
            context.Init<RollbackTags>(
                new { context.Message.Message.CorrelationId }
            )
        ).PublishAsync(context =>
            context.Init<RollbackContent>(
                new { context.Message.Message.CorrelationId }
            )
        ).PublishAsync(context =>
            context.Init<RollbackMentions>(
                new { context.Message.Message.CorrelationId }
            )
        );
    }

    public static EventActivityBinder<NewKweetState> PublishRevertProcessing(
        this EventActivityBinder<NewKweetState> binder)
    {
        return binder.PublishAsync(context =>
            context.Init<RollbackTags>(
                new { context.Saga.CorrelationId }
            )
        ).PublishAsync(context =>
            context.Init<RollbackContent>(
                new { context.Saga.CorrelationId }
            )
        ).PublishAsync(context =>
            context.Init<RollbackMentions>(
                new { context.Saga.CorrelationId }
            )
        );
    }

    public static EventActivityBinder<NewKweetState, Fault<MentionCompleted>> PublishRevertContent(
        this EventActivityBinder<NewKweetState, Fault<MentionCompleted>> binder)
    {
        return binder.PublishAsync(context =>
            context.Init<Fault<ContentCompleted>>(
                new { context.Message.Message.CorrelationId }
            )
        );
    }

    public static EventActivityBinder<NewKweetState, ProcessingRolledBack> PublishRevertTextProcessing(
        this EventActivityBinder<NewKweetState, ProcessingRolledBack> binder)
    {
        return binder.PublishAsync(context =>
            context.Init<Fault<TextCompleted>>(
                new { context.Message.CorrelationId }
            )
        );
    }

    public static EventActivityBinder<NewKweetState> PublishRevertTextProcessing(
        this EventActivityBinder<NewKweetState> binder)
    {
        return binder.PublishAsync(context =>
            context.Init<Fault<TextCompleted>>(
                new { context.Saga.CorrelationId }
            )
        );
    }

    public static EventActivityBinder<NewKweetState, Fault<TextCommand>> PublishKweetPostFailed(
        this EventActivityBinder<NewKweetState, Fault<TextCommand>> binder)
    {
        return binder.PublishAsync(context =>
            context.Init<Fault<NewKweet>>(
                new { context.Message.Message.CorrelationId }
            )
        );
    }

    public static EventActivityBinder<NewKweetState> PublishKweetPostFailed(
        this EventActivityBinder<NewKweetState> binder)
    {
        return binder.PublishAsync(context =>
            context.Init<Fault<NewKweet>>(
                new { context.Saga.CorrelationId }
            )
        );
    }

    public static EventActivityBinder<NewKweetState> PublishTagRollback(
        this EventActivityBinder<NewKweetState> binder)
    {
        return binder.PublishAsync(
            context => context.Init<RollbackTags>(
                new { context.CorrelationId }));
    }

    public static EventActivityBinder<NewKweetState> PublishContentRollback(
        this EventActivityBinder<NewKweetState> binder)
    {
        return binder.PublishAsync(
            context => context.Init<RollbackContent>(
                new { context.CorrelationId }));
    }

    public static EventActivityBinder<NewKweetState> PublishMentionRollback(
        this EventActivityBinder<NewKweetState> binder)
    {
        return binder.PublishAsync(
            context => context.Init<RollbackMentions>(
                new { context.CorrelationId }));
    }
}