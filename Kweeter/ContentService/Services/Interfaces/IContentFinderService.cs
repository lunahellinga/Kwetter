using System;
using System.Collections.Generic;

namespace ContentService.Services.Interfaces;

public interface IContentFinderService
{
    public List<string> GetContent(Guid user, string input);
}