using System.Collections.Generic;
using Domain.Events;
using Kweeter.StateMachines.States;
using MassTransit;

namespace Kweeter.StateMachines.Binders;

public static class InternalPublishers
{
    public static EventActivityBinder<NewKweetState, Fault<TagCommand>> PublishFinishProcessingThenRevert(
        this EventActivityBinder<NewKweetState, Fault<TagCommand>> binder)
    {
        return binder.PublishAsync(context =>
            context.Init<TagCompleted>(
                new TagCompleted
                {
                    CorrelationId = context.Saga.CorrelationId,
                    TagCount = 0,
                    Tags = new List<string>()
                }
            )
        );
    }

    public static EventActivityBinder<NewKweetState, Fault<ContentCommand>> PublishFinishProcessingThenRevert(
        this EventActivityBinder<NewKweetState, Fault<ContentCommand>> binder)
    {
        return binder.PublishAsync(context =>
            context.Init<ContentCompleted>(
                new ContentCompleted
                {
                    CorrelationId = context.Saga.CorrelationId,
                    ContentCount = 0,
                    ContentLinks = new List<string>()
                }
            )
        );
    }

    public static EventActivityBinder<NewKweetState, Fault<MentionCommand>> PublishFinishProcessingThenRevert(
        this EventActivityBinder<NewKweetState, Fault<MentionCommand>> binder)
    {
        return binder.PublishAsync(context =>
            context.Init<MentionCompleted>(
                new MentionCompleted
                {
                    CorrelationId = context.Saga.CorrelationId,
                    MentionCount = 0,
                    MentionedUsers = new List<string>()
                }
            )
        );
    }
}