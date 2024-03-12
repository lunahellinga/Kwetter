using System;
using System.Reflection;
using System.Threading.Tasks;
using Kweeter.Extensions;
using Kweeter.Settings;
using MassTransit;
using MassTransit.Configuration;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Serilog;

namespace Kweeter;

public class Program
{
    public static async Task Main(string[] args)
    {
        await CreateHostBuilder(args).Build().RunAsync();
        await Log.CloseAndFlushAsync();
    }

    public static IHostBuilder CreateHostBuilder(string[] args)
    {
        return Host.CreateDefaultBuilder(args)
            .AddSerilog()
            .ConfigureAppConfiguration(builder =>
            {
                var configurationBuilder = new ConfigurationBuilder();
                var configuration = configurationBuilder.AddEnvironmentVariables().AddJsonFile("appsettings.json")
                    .AddJsonFile($"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")}.json")
                    .Build();

                builder.Sources.Clear();
                builder.AddConfiguration(configuration);
            })
            .ConfigureServices((hostContext, services) =>
            {
                // var messageBrokerQueueSettings = hostContext.Configuration.GetSection("MessageBroker:QueueSettings")
                //     .Get<MessageBrokerQueueSettings>();
                var messageBrokerQueueSettings = new MessageBrokerQueueSettings
                {
                    HostName = Environment.GetEnvironmentVariable("RABBITMQ_HOST") ?? "rabbitmq",
                    Port = int.Parse(Environment.GetEnvironmentVariable("RABBITMQ_PORT") ?? "5672"),
                    VirtualHost = Environment.GetEnvironmentVariable("RABBITMQ_VIRTUAL_HOST") ?? "/",
                    UserName = Environment.GetEnvironmentVariable("RABBITMQ_USERNAME") ?? "rabbit",
                    Password = Environment.GetEnvironmentVariable("RABBITMQ_PASSWORD") ?? "rabbit"
                };
                // var messageBrokerPersistenceSettings = hostContext.Configuration
                //     .GetSection("MessageBroker:StateMachinePersistence").Get<MessageBrokerPersistenceSettings>();
                var messageBrokerPersistenceSettings = new MessageBrokerPersistenceSettings
                {
                    Connection = Environment.GetEnvironmentVariable("MONGODB_CONNECTION") ??
                                 "mongodb://mongodb:27017/",
                    DatabaseName = Environment.GetEnvironmentVariable("MONGODB_DATABASE") ?? "kweeter",
                    CollectionName = Environment.GetEnvironmentVariable("MONGODB_COLLECTION") ?? "saga"
                };

                services.AddMassTransit(x =>
                {
                    x.AddDelayedMessageScheduler();
                    x.SetKebabCaseEndpointNameFormatter();

                    x.SetSagaRepositoryProvider(new MongoDbSagaRepositoryRegistrationProvider(r =>
                    {
                        r.Connection = messageBrokerPersistenceSettings.Connection;
                        r.DatabaseName = messageBrokerPersistenceSettings.DatabaseName;
                        r.CollectionName = messageBrokerPersistenceSettings.CollectionName;
                    }));

                    var entryAssembly = Assembly.GetEntryAssembly();

                    x.AddSagaStateMachines(entryAssembly);
                    x.AddSagas(entryAssembly);
                    x.AddActivities(entryAssembly);

                    x.UsingRabbitMq((context, cfg) =>
                    {
                        cfg.Host(messageBrokerQueueSettings.HostName, messageBrokerQueueSettings.VirtualHost, h =>
                        {
                            h.Username(messageBrokerQueueSettings.UserName);
                            h.Password(messageBrokerQueueSettings.Password);
                        });

                        cfg.UseDelayedMessageScheduler();
                        cfg.ConfigureEndpoints(context);
                    });
                });
            });
    }
}