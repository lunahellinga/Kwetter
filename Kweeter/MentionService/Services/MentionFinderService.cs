using System;
using System.Collections.Generic;
using MentionService.Services.Interfaces;

namespace MentionService.Services;

public class MentionFinderService : IMentionFinderService
{
    public List<string> FindMentions(Guid user, string input)
    {
        return new List<string>();
    }
}