namespace Domain.Models;

public class Kweet
{
    public Guid KweetId { get; set; }

    public Guid UserId { get; set; }
    public string UserName { get; set; }
    public string KweetText { get; set; }
    public List<string> Tags { get; set; }
    public List<string> MentionedUsers { get; set; }
    public List<string> ContentLinks { get; set; }
    public DateTime CreatedAt { get; set; }
}