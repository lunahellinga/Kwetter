namespace Postgres.Models;

public class KweetDto
{
    public KweetDto(Guid id, Guid user, string message)
    {
        Id = id;
        User = user;
        Message = message;
    }

    public Guid Id { get; set; }
    public Guid User { get; set; }
    public string Message { get; set; }
    
}