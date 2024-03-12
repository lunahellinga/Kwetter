using Neo4j.Driver.Extensions;

namespace Neo4j.Models;

public class Kweet
{
    [Neo4jProperty(Name = "id")] public string Id { get; set; }

    [Neo4jProperty(Name = "user")] public string User { get; set; }

    [Neo4jProperty(Name = "message")] public string Message { get; set; }

    [Neo4jProperty(Name = "timestamp")] public DateTime Timestamp { get; set; }
    
    public Kweet(Guid id, Guid user, string message)
    {
        Id = id.ToString();
        User = user.ToString();
        Message = message;
        Timestamp = DateTime.UtcNow;
    }

    public Kweet(string id, string user, string message, DateTime timestamp)
    {
        Id = id;
        User = user;
        Message = message;
        Timestamp = timestamp;
    }
    

    public Kweet()
    {
    }
}