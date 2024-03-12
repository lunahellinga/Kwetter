using System;
using System.Collections.Generic;

namespace MentionService.Services.Interfaces;

public interface IMentionFinderService
{
    public List<string> FindMentions(Guid user, string input);
}