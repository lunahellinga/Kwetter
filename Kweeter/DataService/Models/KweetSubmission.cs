using System;

namespace DataService.Models;

public class KweetSubmission
{
    public KweetSubmission(Guid id, string message)
    {
        Id = id;
        Message = message;
    }

    public Guid Id { get; set; }
    public string Message { get; set; }
}