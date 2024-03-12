using System;
using System.Collections.Generic;
using ContentService.Services.Interfaces;

namespace ContentService.Services;

public class ContentFinderService : IContentFinderService
{
    public List<string> GetContent(Guid user, string input)
    {
        return new List<string>();
    }
}