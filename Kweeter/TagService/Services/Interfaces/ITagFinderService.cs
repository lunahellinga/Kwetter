using System;
using System.Collections.Generic;
using TagService.Models;

namespace TagService.Services.Interfaces;

public interface ITagFinderService
{
    public List<Tag> FindTags(Guid user, string input);
}