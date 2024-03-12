using Domain.Events;
using Kweeter.StateMachines.Binders;
using Kweeter.StateMachines.States;
using MassTransit;
using Serilog;

namespace Kweeter.StateMachines;

public class PostStateMachine :
    MassTransitStateMachine<NewKweetState>
{
    public PostStateMachine()
    {
        /*
         * Event is registered
         *  Execute logic on message
         *  Publish event to queue
         *  Enter state (initial or .TransitionTo)
         * Consumer does stuff
         *  Publishes XxxxCompleted event
         * Use During(When(Event)) to trigger off of that event
         *  Specify how to do correlation for that event via Event(()=>Event, => ...)
         *  Loop
         *
         * If consumer fails it publishes XxxxFailed event
         *  Use DuringAny(When) to trigger off of that
         *  Publish a Fault<PreviousEvent>
         * Consumer triggers its rollback off of that
         *  Publishes XxxxRollbackCompleted
         * Trigger next rollback step in state machine off of that
         * Run the loop backwards until everything is undone
         */

        InstanceState(x => x.CurrentState);

        #region Event Declarations

        // New kweet is posted
        Event(() => NewKweet, e =>
        {
            e.InsertOnInitial = true;
            e.SetSagaFactory(context => new NewKweetState
            {
                CorrelationId = context.Message.CorrelationId
            });
        });
        // Event(() => NewKweetFault,
        //     x => x.CorrelateById(context => context.Message.Message.CorrelationId));
        // Text is processed
        // Event(() => TextCommand);
        Event(() => TextCommandFault,
            x => x.CorrelateById(context => context.Message.Message.CorrelationId));
        Event(() => TextCompleted);
        // Event(() => RollbackText);
        // Split processing
        Event(() => ProcessingRolledBack);
        // Event(() => EvaluateProcessing);
        // Content
        // Event(() => ContentCommand);
        Event(() => ContentCommandFault,
            x => x.CorrelateById(context => context.Message.Message.CorrelationId));
        Event(() => ContentCompleted);
        // Event(() => RollbackContent);
        // Tag
        // Event(() => TagCommand);
        Event(() => TagCommandFault,
            x => x.CorrelateById(context => context.Message.Message.CorrelationId));
        Event(() => TagCompleted);
        // Event(() => RollbackTags);
        // Mention
        // Event(() => MentionCommand);
        Event(() => MentionCommandFault,
            x => x.CorrelateById(context => context.Message.Message.CorrelationId));
        Event(() => MentionCompleted);
        // Event(() => RollbackMentions);
        // Metadata
        // Event(() => MetadataCommand);
        Event(() => MetadataCommandFault,
            x => x.CorrelateById(context => context.Message.Message.CorrelationId));
        // Event(() => RollbackMetadata);
        // Cache
        // Event(() => CacheCommand);
        Event(() => CacheCommandFault,
            x => x.CorrelateById(context => context.Message.Message.CorrelationId));
        Event(() => CacheCompleted);

        #endregion

        #region Flow

        // Tweet is created and sent to text storage
        Initially(
            When(NewKweet)
                .Then(c => Log.Logger.Information(
                    "Received new kweet: {MessageCorrelationId} | Text: {MessageKweetText}", c.Message.CorrelationId,
                    c.Message.KweetText))
                .SetInitialKweetData()
                .TransitionTo(ProcessingText)
                .PublishProcessTextCommand()
        );
        // Text is stored and sent for processing for tags, mentions and content
        During(ProcessingText,
            When(TextCompleted)
                .Then(c => Log.Logger.Information(
                    "Kweet has been stored, sending to processing: {MessageCorrelationId}", c.Message.CorrelationId))
                .SetSanitizedText()
                .TransitionTo(Processing)
                .PublishProcessingCommands()
        );
        // The parallel steps are handled by composites in the "Composite Events" section
        During(Processing, When(EvaluateProcessingTrigger)
            .Then(c => Log.Logger.Information(
                "Kweet has been processed, evaluating processing status: {SagaCorrelationId} | Failures: C {SagaContentFailed} - M {SagaMentionsFailed} - T {SagaTagsFailed}",
                c.Saga.CorrelationId, c.Saga.ContentFailed, c.Saga.MentionsFailed, c.Saga.TagsFailed))
            .If(c => c.Saga is { ContentFailed: false, MentionsFailed: false, TagsFailed: false },
                c => c
                    .Then(y => Log.Logger.Information(
                        "Kweet processing went well, moving to metadata: {SagaCorrelationId} | Tags: {SagaTags} | Mentions: {SagaMentionedUsers} | Content: {SagaContentLinks}",
                        y.Saga.CorrelationId, y.Saga.Tags, y.Saga.MentionedUsers, y.Saga.ContentLinks))
                    .TransitionTo(Metadata)
                    .PublishMetadataCommand()));
        // .TransitionTo(Evaluating));
        // Then, if none of the processing steps failed, we go to proceed to store metadata
        // WhenEnter(Evaluating, x
        //     => x.If(c => c.Saga is { ContentFailed: false, MentionsFailed: false, TagsFailed: false },
        //         c => c
        //             .Then(y => Log.Logger.Information(
        //                 $"Kweet processing went well, moving to metadata: {y.Saga.CorrelationId} | {y.Saga.Tags} | {y.Saga.MentionedUsers} | {y.Saga.ContentLinks}"))
        //             .TransitionTo(Metadata)
        //             .PublishMetadataCommand()));
        // We then cache it
        During(Metadata,
            When(MetadataStored)
                .SetCreationDate()
                .Then(y => Log.Logger.Information(
                    "Kweet metadata stored, caching: {SagaCorrelationId} | Tag count: {SagaTagCount} | Mention count: {SagaMentionCount} | Content count: {SagaContentCount}",
                    y.Saga.CorrelationId, y.Saga.TagCount, y.Saga.MentionCount, y.Saga.ContentCount))
                .TransitionTo(Caching)
                .PublishCacheCommand()
        );
        // And if it caches we're done
        During(Caching,
            When(CacheCompleted)
                .Then(y => Log.Logger.Information("Kweet {KweetId} cached! Have fun reading it :)",
                    y.Saga.CorrelationId))
                .Finalize()
        );
        SetCompletedWhenFinalized();

        #endregion

        #region Fault-Companse State

        // Caching faults, revert metadata
        DuringAny(When(CacheCommandFault)
            .TransitionTo(CachingFaulted)
            .PublishRevertMetadata());

        // Metadata faults, revert all processing steps
        DuringAny(When(MetadataCommandFault)
            .TransitionTo(MetadataFaulted)
            .PublishRevertProcessing()
        );

        // If any of the processing steps fault, we tag it and continue on. We'll handle this in the Evaluate Processing step
        DuringAny(When(TagCommandFault)
            .Then(c => c.Saga.TagsFailed = true)
            .PublishFinishProcessingThenRevert());
        DuringAny(When(MentionCommandFault)
            .Then(c => c.Saga.MentionsFailed = true)
            .PublishFinishProcessingThenRevert());
        DuringAny(When(ContentCommandFault)
            .Then(c => c.Saga.ContentFailed = true)
            .PublishFinishProcessingThenRevert());

        // If a step didn't fail, but one of the others did, the successful step has to be rolled back
        During(Processing, When(EvaluateProcessingTrigger).If(
            c => (c.Saga.ContentFailed || c.Saga.MentionsFailed) && !c.Saga.TagsFailed,
            c => c.PublishTagRollback()));
        During(Processing, When(EvaluateProcessingTrigger).If(
            c => (c.Saga.ContentFailed || c.Saga.TagsFailed) && !c.Saga.MentionsFailed,
            c => c.PublishMentionRollback()));
        During(Processing, When(EvaluateProcessingTrigger).If(
            c => (c.Saga.TagsFailed || c.Saga.MentionsFailed) && !c.Saga.ContentFailed,
            c => c.PublishContentRollback()));

        // WhenEnter(Evaluating, x
        //     => x.If(c => (c.Saga.ContentFailed || c.Saga.MentionsFailed) && !c.Saga.TagsFailed,
        //         c => c.PublishTagRollback()));
        // WhenEnter(Evaluating, x
        //     => x.If(c => (c.Saga.ContentFailed || c.Saga.TagsFailed) && !c.Saga.MentionsFailed,
        //         c => c.PublishMentionRollback()));
        // WhenEnter(Evaluating, x
        //     => x.If(c => (c.Saga.TagsFailed || c.Saga.MentionsFailed) && !c.Saga.ContentFailed,
        //         c => c.PublishContentRollback()));

        // If all processing steps have been rolled back, we move to roll back text storage
        DuringAny(When(ProcessingRolledBackTrigger)
            .TransitionTo(ProcessingFaulted)
            .PublishRevertTextProcessing());

        // If text step faults we're at the start of the process
        DuringAny(When(TextCommandFault)
            .PublishKweetPostFailed()
            .Finalize()
        );

        // Full WhenEnter flow
        WhenEnter(CachingFaulted,
            c => c.PublishRevertProcessing()
                .TransitionTo(MetadataFaulted));
        WhenEnter(MetadataFaulted,
            c => c
                .PublishTagRollback()
                .PublishContentRollback()
                .PublishMentionRollback()
                .TransitionTo(ProcessingFaulted)
        );
        // WhenEnter(Evaluating, x
        //     => x.If(c => c.Saga.TagsFailed || c.Saga.MentionsFailed || c.Saga.ContentFailed,
        //         c => c.TransitionTo(ProcessingFaulted)));
        WhenEnter(ProcessingFaulted,
            c => c.PublishRevertTextProcessing().TransitionTo(ProcessingTextFaulted));
        WhenEnter(ProcessingTextFaulted, c =>
            c.PublishKweetPostFailed().Finalize());

        During(Processing, When(EvaluateProcessingTrigger).If(
            c => c.Saga.TagsFailed || c.Saga.MentionsFailed || c.Saga.ContentFailed,
            c => c.TransitionTo(ProcessingFaulted)));

        #endregion


        During(Processing,
            When(TagCompleted).Then(c => Log.Logger.Information("Tag completed for {Kweet}", c.Saga.ContentFailed)));
        During(Processing,
            When(ContentCompleted)
                .Then(c => Log.Logger.Information("Content completed for {Kweet}", c.Saga.ContentFailed)));
        During(Processing,
            When(MentionCompleted)
                .Then(c => Log.Logger.Information("Mention completed for {Kweet}", c.Saga.ContentFailed)));

        CompositeEvent(() => EvaluateProcessingTrigger,
            x => x.EvaluateProcessStatus,
            CompositeEventOptions.RaiseOnce,
            TagCompleted, ContentCompleted, MentionCompleted
        );
        CompositeEvent(() => ProcessingRolledBackTrigger,
            x => x.RevertProcessStatus,
            CompositeEventOptions.RaiseOnce,
            TagCommandFault, ContentCommandFault, MentionCommandFault
        );
        SetCompletedWhenFinalized();

        // During(Processing, Ignore(TagCompleted));
        // During(Processing, Ignore(ContentCompleted));
        // During(Processing, Ignore(MentionCompleted));
        During(Processing, Ignore(TagCommandFault));
        During(Processing, Ignore(ContentCommandFault));
        During(Processing, Ignore(MentionCommandFault));
    }

    // Store all states in here

    #region States

    public State ProcessingText { get; }
    public State ProcessingTextFaulted { get; }

    public State Processing { get; }
    public State ProcessingFaulted { get; }

    public State Evaluating { get; }
    public State Metadata { get; }
    public State MetadataFaulted { get; }

    public State Caching { get; }
    public State CachingFaulted { get; }

    #endregion

    // Store all event instances here, event records in the Domain/Events

    #region Events

    public Event<NewKweet> NewKweet { get; }

    // public Event<Fault<NewKweet>> NewKweetFault { get; private set; }

    // Text
    // public Event<TextCommand> TextCommand { get; private set; }
    public Event<Fault<TextCommand>> TextCommandFault { get; }

    public Event<TextCompleted> TextCompleted { get; }
    // public Event<RollbackText> RollbackText { get; private set; }

    // Split processing
    // public Event<ContentCommand> ContentCommand { get; private set; }
    public Event<Fault<ContentCommand>> ContentCommandFault { get; }
    public Event<ContentCompleted> ContentCompleted { get; }

    // public Event<MentionCommand> MentionCommand { get; private set; }
    public Event<Fault<MentionCommand>> MentionCommandFault { get; }
    public Event<MentionCompleted> MentionCompleted { get; }

    // public Event<TagCommand> TagCommand { get; private set; }
    public Event<Fault<TagCommand>> TagCommandFault { get; }
    public Event<TagCompleted> TagCompleted { get; }

    // public Event EvaluateProcessing { get; private set; }
    public Event EvaluateProcessingTrigger { get; }

    // Rollbacks for split stage
    public Event<ProcessingRolledBack> ProcessingRolledBack { get; }

    public Event ProcessingRolledBackTrigger { get; }
    // public Event<RollbackTags> RollbackTags { get; private set; }
    // public Event<RollbackMentions> RollbackMentions { get; private set; }
    // public Event<RollbackContent> RollbackContent { get; private set; }

    // Metadata
    // public Event<MetadataCommand> MetadataCommand { get; private set; }
    public Event<Fault<MetadataCommand>> MetadataCommandFault { get; }
    public Event<MetadataCompleted> MetadataStored { get; }

    // public Event<RollbackMetadata> RollbackMetadata { get; private set; }

    // Cache
    // public Event<CacheCommand> CacheCommand { get; private set; }
    public Event<Fault<CacheCommand>> CacheCommandFault { get; }
    public Event<CacheCompleted> CacheCompleted { get; }

    #endregion
}