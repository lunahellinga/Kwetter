using System;
using System.Text.RegularExpressions;
using DataService.Services.Interfaces;
using Ganss.Xss;
using Serilog;

namespace DataService.Services;

public partial class SanitizerService : ISanitizerService
{
    /// <summary>
    ///     Sanitize the string, removing or escaping any potential script or sql injection.
    /// </summary>
    /// <param name="user"></param>
    /// <param name="input"></param>
    /// <returns></returns>
    public string Sanitize(Guid user, string input)
    {
        // Escape xss scripts
        var sanitizer = new HtmlSanitizer();
        var output = sanitizer.Sanitize(input);
        if (output != input) Log.Logger.Information("SEC: Sanitized {User} message: {Input}", user, input);
        output = output.Trim();
        output = WhitespaceRemoval().Replace(output, "$1");

        return output;
    }

    public bool Validate(Guid user, string input)
    {
        if (input.Length > 140) return false;

        return true;
    }

    [GeneratedRegex("(\\s)\\s+")]
    private static partial Regex WhitespaceRemoval();
}