using System;
using System.Collections.Generic;
using MassTransit;

namespace Kweeter.StateMachines.States;

public class NewKweetState :
    SagaStateMachineInstance, ISagaVersion
{
    public int CurrentState { get; set; }

    public Guid UserId { get; set; }
    public string UserName { get; set; }

    public string KweetText { get; set; }

    public int TagCount { get; set; }
    public List<string> Tags { get; set; }

    public int MentionCount { get; set; }
    public List<string> MentionedUsers { get; set; }

    public int ContentCount { get; set; }
    public List<string> ContentLinks { get; set; }
    public DateTime CreatedAt { get; set; }

    public int RevertProcessStatus { get; set; }
    public int EvaluateProcessStatus { get; set; }
    public bool TagsFailed { get; set; }
    public bool MentionsFailed { get; set; }
    public bool ContentFailed { get; set; }

    public int Version { get; set; }

    public Guid CorrelationId { get; set; }
}