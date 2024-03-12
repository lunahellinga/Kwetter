using Domain.Events;
using Kweeter.StateMachines.States;
using MassTransit;

namespace Kweeter.StateMachines.Binders;

public static class Setters
{
    public static EventActivityBinder<NewKweetState, NewKweet> SetInitialKweetData(
        this EventActivityBinder<NewKweetState, NewKweet> binder)
    {
        return binder.Then(context =>
        {
            context.Saga.UserId = context.Message.UserId;
            context.Saga.UserName = context.Message.UserName;
            context.Saga.KweetText = context.Message.KweetText;
        });
    }

    public static EventActivityBinder<NewKweetState, TextCompleted> SetSanitizedText(
        this EventActivityBinder<NewKweetState, TextCompleted> binder)
    {
        return binder.Then(context => { context.Saga.KweetText = context.Message.KweetText; });
    }

    public static EventActivityBinder<NewKweetState, MentionCompleted> SetMentions(
        this EventActivityBinder<NewKweetState, MentionCompleted> binder)
    {
        return binder.Then(context =>
        {
            context.Saga.MentionCount = context.Message.MentionCount;
            context.Saga.MentionedUsers = context.Message.MentionedUsers;
        });
    }

    public static EventActivityBinder<NewKweetState, TagCompleted> SetTags(
        this EventActivityBinder<NewKweetState, TagCompleted> binder)
    {
        return binder.Then(context =>
        {
            context.Saga.TagCount = context.Message.TagCount;
            context.Saga.Tags = context.Message.Tags;
        });
    }

    public static EventActivityBinder<NewKweetState, ContentCompleted> SetContent(
        this EventActivityBinder<NewKweetState, ContentCompleted> binder)
    {
        return binder.Then(context =>
        {
            context.Saga.ContentCount = context.Message.ContentCount;
            context.Saga.ContentLinks = context.Message.ContentLinks;
        });
    }

    public static EventActivityBinder<NewKweetState, MetadataCompleted> SetCreationDate(
        this EventActivityBinder<NewKweetState, MetadataCompleted> binder)
    {
        return binder.Then(context => { context.Saga.CreatedAt = context.Message.CreatedAt; });
    }
}