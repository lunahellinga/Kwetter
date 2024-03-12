using System;
using System.Collections.Generic;
using TagService.Models;
using TagService.Services.Interfaces;

namespace TagService.Services;

public class TagFinderService : ITagFinderService
{
    public List<Tag> FindTags(Guid user, string input)
    {
        return new List<Tag>();
    }
}