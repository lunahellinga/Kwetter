namespace Postgres.Models;

public class Kweet
{
    public Guid Id { get; set; }

    public Guid User { get; set; }
    public string Message { get; set; }
    public DateTime Timestamp { get; set; }

    public Kweet(Guid id, Guid user, string message)
    {
        Id = id;
        User = user;
        Message = message;
        Timestamp = DateTime.UtcNow;
    }
}