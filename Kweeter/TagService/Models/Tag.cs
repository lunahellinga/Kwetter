using System;
using System.Collections.Generic;

namespace TagService.Models;

public class Tag
{
    public Guid Id { get; set; }
    public string Text { get; set; }
    public ICollection<TagUse> TagUses { get; set; }
}