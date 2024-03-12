using System;

namespace DataService.Services.Interfaces;

public interface ISanitizerService
{
    public string Sanitize(Guid user, string input);
    public bool Validate(Guid user, string input);
}