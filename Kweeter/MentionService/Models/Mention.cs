using System;

namespace MentionService.Models;

public class Mention
{
    public Guid Id { get; set; }
    public Guid KweetId { get; set; }
    public Guid UserId { get; set; }
}