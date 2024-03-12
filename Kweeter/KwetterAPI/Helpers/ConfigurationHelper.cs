using System.Reflection;
using Kweeter.Settings;
using MassTransit;

namespace KwetterAPI.Helpers;

public static class ConfigurationHelper
{
    public static void AddRabbitMq(WebApplicationBuilder webApplicationBuilder)
    {
        var messageBrokerQueueSettings = new MessageBrokerQueueSettings
        {
            HostName = Environment.GetEnvironmentVariable("RABBITMQ_HOST") ?? "rabbitmq",
            Port = int.Parse(Environment.GetEnvironmentVariable("RABBITMQ_PORT") ?? "5672"),
            VirtualHost = Environment.GetEnvironmentVariable("RABBITMQ_VIRTUAL_HOST") ?? "/",
            UserName = Environment.GetEnvironmentVariable("RABBITMQ_USERNAME") ?? "rabbit",
            Password = Environment.GetEnvironmentVariable("RABBITMQ_PASSWORD") ?? "rabbit"
        };

        webApplicationBuilder.Services.AddMassTransit(mt => mt.AddMassTransit(x =>
        {
            x.AddDelayedMessageScheduler();

            x.SetKebabCaseEndpointNameFormatter();

            var entryAssembly = Assembly.GetEntryAssembly();

            x.AddConsumers(entryAssembly);
            x.AddActivities(entryAssembly);

            x.UsingRabbitMq((ctx, cfg) =>
            {
                cfg.Host(messageBrokerQueueSettings.HostName, messageBrokerQueueSettings.VirtualHost, h =>
                {
                    h.Username(messageBrokerQueueSettings.UserName);
                    h.Password(messageBrokerQueueSettings.Password);
                });

                cfg.UseDelayedMessageScheduler();

                cfg.ConfigureEndpoints(ctx);
            });
        }));
    }
}