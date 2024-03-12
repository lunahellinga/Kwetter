using Microsoft.Extensions.Hosting;
using Serilog;
using Serilog.Sinks.SystemConsole.Themes;

namespace Kweeter.Extensions;

public static class ServiceCollectionExtensions
{
    public static IHostBuilder AddSerilog(this IHostBuilder builder)
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

        Log.Logger = new LoggerConfiguration()
            .MinimumLevel.Information()
            // .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
            // .MinimumLevel.Override("System", LogEventLevel.Warning)
            .WriteTo.Console(
                outputTemplate:
                "[{Timestamp:yyyy-MM-dd HH:mm:ss} {Level}] {SourceContext}{NewLine}{Message:lj}{NewLine}{Exception}{NewLine}",
                theme: AnsiConsoleTheme.Literate)
            .CreateLogger();

        return builder;
    }
}