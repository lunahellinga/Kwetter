using System;

namespace ContentService.Models;

public class Content
{
    public Content(Guid kweetId, string link)
    {
        Id = Guid.NewGuid();
        KweetId = kweetId;
        Link = link;
    }

    public Guid Id { get; set; }
    public Guid KweetId { get; set; }
    public string Link { get; set; }
}