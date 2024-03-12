using System;
using System.Reflection;
using System.Threading.Tasks;
using ContentService.Data;
using ContentService.Extensions;
using ContentService.Services;
using ContentService.Services.Interfaces;
using EntityFramework.Exceptions.PostgreSQL;
using Kweeter.Settings;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Npgsql;
using Serilog;

namespace ContentService;

public class Program
{
    public static async Task Main(string[] args)
    {
        var app = CreateHostBuilder(args).Build();

        using (var scope = app.Services.CreateScope())
        {
            var db = scope.ServiceProvider.GetRequiredService<DataContext>();
            await db.Database.MigrateAsync();
        }

        await app.RunAsync();
        await Log.CloseAndFlushAsync();
    }

    public static IHostBuilder CreateHostBuilder(string[] args)
    {
        return Host.CreateDefaultBuilder(args)
            .AddSerilog()
            .ConfigureAppConfiguration((hostContext, config) =>
            {
                config
                    // .AddKeyPerFile("/run/secrets", true)
                    // .SetBasePath(Directory.GetCurrentDirectory())
                    .AddJsonFile("appsettings.json")
                    .AddJsonFile(
                        $"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Production"}.json",
                        true)
                    .AddEnvironmentVariables()
                    .Build();
            })
            .ConfigureServices((hostContext, services) =>
                {
                    using var log = new LoggerConfiguration()
                        .ReadFrom.Configuration(hostContext.Configuration)
                        .CreateLogger();
                    Log.Logger = log;

                    // var connectionString = new NpgsqlConnectionStringBuilder
                    // {
                    //     Host = hostContext.Configuration.GetValue<string>("yugabyte_host"),
                    //     Port = 5433,
                    //     Database = Environment.GetEnvironmentVariable("DATABASE"),
                    //     Username = hostContext.Configuration.GetValue<string>("yugabyte_user"),
                    //     Password = hostContext.Configuration.GetValue<string>("yugabyte_password"),
                    //     SslMode = SslMode.VerifyFull,
                    //     RootCertificate = Environment.GetEnvironmentVariable("ROOT_CERT")
                    // }.ConnectionString;
                    if (Convert.ToBoolean(Environment.GetEnvironmentVariable("YUGA")))
                    {
                        var connectionString = new NpgsqlConnectionStringBuilder
                        {
                            Host = Environment.GetEnvironmentVariable("YUGABYTE_HOST") ??
                                   throw new InvalidOperationException("Missing YUGABYTE_HOST"),
                            Port = 5433,
                            Database = Environment.GetEnvironmentVariable("DATABASE"),
                            Username = Environment.GetEnvironmentVariable("YUGABYTE_USER") ??
                                       throw new InvalidOperationException("Missing YUGABYTE_USER"),
                            Password = Environment.GetEnvironmentVariable("YUGABYTE_PASSWORD") ??
                                       throw new InvalidOperationException("Missing YUGABYTE_PASSWORD"),
                            SslMode = SslMode.VerifyFull,
                            RootCertificate = Environment.GetEnvironmentVariable("ROOT_CERT")
                        }.ConnectionString;
                        services.AddDbContext<DataContext>(options =>
                        {
                            options.UseNpgsql(connectionString);
                            options.UseExceptionProcessor();
                        });
                    }
                    else
                    {
                        services.AddDbContext<DataContext>(options =>
                        {
                            // options.UseNpgsql(builder.Configuration.GetConnectionString("Default"));
                            options.UseNpgsql(Environment.GetEnvironmentVariable("CONNECTION_STRING"));
                        });
                    }

                    var messageBrokerQueueSettings = new MessageBrokerQueueSettings
                    {
                        HostName = Environment.GetEnvironmentVariable("RABBITMQ_HOST") ?? "rabbitmq",
                        Port = int.Parse(Environment.GetEnvironmentVariable("RABBITMQ_PORT") ?? "5672"),
                        VirtualHost = Environment.GetEnvironmentVariable("RABBITMQ_VIRTUAL_HOST") ?? "/",
                        UserName = Environment.GetEnvironmentVariable("RABBITMQ_USERNAME") ?? "rabbit",
                        Password = Environment.GetEnvironmentVariable("RABBITMQ_PASSWORD") ?? "rabbit"
                    };

                    services.AddMassTransit(x =>
                        {
                            x.AddDelayedMessageScheduler();

                            x.SetKebabCaseEndpointNameFormatter();

                            var entryAssembly = Assembly.GetEntryAssembly();

                            x.AddConsumers(entryAssembly);
                            x.AddActivities(entryAssembly);

                            x.UsingRabbitMq((context, cfg) =>
                                {
                                    cfg.Host(messageBrokerQueueSettings.HostName,
                                        messageBrokerQueueSettings.VirtualHost, h =>
                                        {
                                            h.Username(messageBrokerQueueSettings.UserName);
                                            h.Password(messageBrokerQueueSettings.Password);
                                        });

                                    cfg.UseDelayedMessageScheduler();

                                    cfg.ConfigureEndpoints(context);
                                }
                            );
                        }
                    );
                    services.AddScoped<IContentManagerService, ContentManagerService>();
                    services.AddScoped<IContentFinderService, ContentFinderService>();
                }
            );
    }
}