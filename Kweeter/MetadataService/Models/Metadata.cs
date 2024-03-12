using System;
using System.ComponentModel.DataAnnotations;

namespace MetadataService.Models;

public class Metadata
{
    [Key] public Guid KweetId { get; set; }

    public Guid UserId { get; init; }
    public int ContentCount { get; init; }
    public int TagCount { get; init; }
    public int MentionCount { get; init; }
    public DateTime CreatedAt { get; set; }
}