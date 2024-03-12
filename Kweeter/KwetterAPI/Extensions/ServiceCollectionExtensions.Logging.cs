using Serilog;
using Serilog.Events;
using Serilog.Filters;
using Serilog.Sinks.SystemConsole.Themes;

namespace KwetterAPI.Extensions;

public static partial class ServiceCollectionExtensions
{
    public static WebApplicationBuilder AddSerilog(this WebApplicationBuilder builder)
    {
        // Log.Logger = new LoggerConfiguration()
        //     .MinimumLevel.Information()
        //     .MinimumLevel.Override("Microsoft.AspNetCore.DataProtection.KeyManagement", LogEventLevel.Warning)
        //     .MinimumLevel.Override("Microsoft.AspNetCore.Authorization", LogEventLevel.Verbose)
        //     .MinimumLevel.Override("System.Net.Http", LogEventLevel.Debug)
        //     .MinimumLevel.Override("Keycloak.AuthServices", LogEventLevel.Verbose)
        //     .WriteTo.SpectreConsole(
        //         "{SourceContext}{NewLine}{Timestamp:HH:mm:ss} [{Level:u4}] {Message:lj}{NewLine}{Exception}",
        //         LogEventLevel.Verbose)
        //     .CreateLogger();

        builder.Host.UseSerilog();

        Log.Logger = new LoggerConfiguration()
            .MinimumLevel.Information()
            .MinimumLevel.Override("Microsoft.AspNetCore.DataProtection.KeyManagement", LogEventLevel.Warning)
            .MinimumLevel.Override("Microsoft.AspNetCore.Authorization", LogEventLevel.Warning)
            // .MinimumLevel.Override("System.Net.Http", LogEventLevel.Warning)
            .MinimumLevel.Override("Keycloak.AuthServices", LogEventLevel.Warning)
            // .MinimumLevel.Override("Microsoft.AspNetCore.Hosting.Diagnostics", LogEventLevel.Warning)
            .Filter.ByExcluding(Matching.WithProperty("RequestPath", "/"))
            .WriteTo.Console(
                outputTemplate:
                "[{Timestamp:yyyy-MM-dd HH:mm:ss} {Level}] {SourceContext}{NewLine}{Message:lj}{NewLine}{Exception}{NewLine}",
                theme: AnsiConsoleTheme.Literate)
            .CreateLogger();

        return builder;
    }
}