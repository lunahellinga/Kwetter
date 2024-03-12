using Domain.Events;
using Kweeter.StateMachines.States;
using MassTransit;

namespace Kweeter.StateMachines.Binders;

public static class Publishers
{
    public static EventActivityBinder<NewKweetState, NewKweet> PublishProcessTextCommand(
        this EventActivityBinder<NewKweetState, NewKweet> binder)
    {
        return binder.PublishAsync(context =>
            context.Init<TextCommand>(
                new TextCommand
                {
                    CorrelationId = context.Saga.CorrelationId,
                    UserId = context.Saga.UserId,
                    KweetText = context.Saga.KweetText
                }
            )
        );
    }

    public static EventActivityBinder<NewKweetState, TextCompleted> PublishProcessingCommands(
        this EventActivityBinder<NewKweetState, TextCompleted> binder)
    {
        return binder.PublishAsync(context =>
            context.Init<TagCommand>(
                new TagCommand
                {
                    CorrelationId = context.Saga.CorrelationId,
                    UserId = context.Saga.UserId,
                    KweetText = context.Saga.KweetText
                }
            )
        ).PublishAsync(context =>
            context.Init<MentionCommand>(
                new MentionCommand
                {
                    CorrelationId = context.Saga.CorrelationId,
                    UserId = context.Saga.UserId,
                    KweetText = context.Saga.KweetText
                }
            )
        ).PublishAsync(context =>
            context.Init<ContentCommand>(
                new ContentCommand
                {
                    CorrelationId = context.Saga.CorrelationId,
                    UserId = context.Saga.UserId,
                    KweetText = context.Saga.KweetText
                }
            )
        );
    }

    public static EventActivityBinder<NewKweetState> PublishMetadataCommand(
        this EventActivityBinder<NewKweetState> binder)
    {
        return binder.PublishAsync(context =>
            context.Init<MetadataCommand>(
                new MetadataCommand
                {
                    CorrelationId = context.Saga.CorrelationId,
                    UserId = context.Saga.UserId,
                    TagCount = context.Saga.TagCount,
                    MentionCount = context.Saga.MentionCount,
                    ContentCount = context.Saga.ContentCount
                }
            )
        );
    }

    public static EventActivityBinder<NewKweetState, MetadataCompleted> PublishCacheCommand(
        this EventActivityBinder<NewKweetState, MetadataCompleted> binder)
    {
        return binder.PublishAsync(context =>
            context.Init<CacheCommand>(
                new CacheCommand
                {
                    CorrelationId = context.Saga.CorrelationId,
                    UserId = context.Saga.UserId,
                    UserName = context.Saga.UserName,
                    KweetText = context.Saga.KweetText,
                    Tags = context.Saga.Tags,
                    MentionedUsers = context.Saga.MentionedUsers,
                    ContentLinks = context.Saga.ContentLinks,
                    CreatedAt = context.Saga.CreatedAt
                }
            )
        );
    }
}