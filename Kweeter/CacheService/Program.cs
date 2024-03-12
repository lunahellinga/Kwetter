using System;
using System.IO;
using System.Reflection;
using CacheService.Data;
using CacheService.Extensions;
using CacheService.Repositories;
using CacheService.Services;
using CacheService.Services.Interfaces;
using Kweeter.Settings;
using MassTransit;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

var builder = WebApplication.CreateBuilder(args);

// Add configuration
builder.Configuration
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json")
    .AddJsonFile(
        $"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Production"}.json",
        true)
    .AddEnvironmentVariables()
    .Build();

// Logging
builder.AddSerilog();
builder.Services.AddHealthChecks();

var messageBrokerQueueSettings = new MessageBrokerQueueSettings
{
    HostName = Environment.GetEnvironmentVariable("RABBITMQ_HOST") ?? "rabbitmq",
    Port = int.Parse(Environment.GetEnvironmentVariable("RABBITMQ_PORT") ?? "5672"),
    VirtualHost = Environment.GetEnvironmentVariable("RABBITMQ_VIRTUAL_HOST") ?? "/",
    UserName = Environment.GetEnvironmentVariable("RABBITMQ_USERNAME") ?? "rabbit",
    Password = Environment.GetEnvironmentVariable("RABBITMQ_PASSWORD") ?? "rabbit"
};

// MassTransit
builder.Services.AddMassTransit(x =>
    {
        x.AddDelayedMessageScheduler();

        x.SetKebabCaseEndpointNameFormatter();

        var entryAssembly = Assembly.GetEntryAssembly();

        x.AddConsumers(entryAssembly);
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
            }
        );
    }
);
// MongoDB
var mongoOptions = new MongoOptions
{
    ConnectionString = Environment.GetEnvironmentVariable("MONGO_CONNECTION"),
    DatabaseName = Environment.GetEnvironmentVariable("MONGO_DATABASE"),
    CollectionName = Environment.GetEnvironmentVariable("MONGO_COLLECTION")
};
var options = Options.Create(mongoOptions);
builder.Services.AddSingleton(options);

//Redis
builder.Services.AddStackExchangeRedisCache(o =>
{
    o.Configuration = Environment.GetEnvironmentVariable("REDIS_CONFIG");
    o.InstanceName = Environment.GetEnvironmentVariable("REDIS_INSTANCE");
});

// Add services to the container.
builder.Services.AddScoped<IKweetWriterRepository, KweetWriterRepository>();
builder.Services.AddScoped<IKweetReaderRepository, KweetReaderRepository>();
builder.Services.AddScoped<IKweetService, KweetService>();
builder.Services.AddScoped<IGdprService, GdprService>();
builder.Services.AddScoped<ICacheManager, CacheManager>();

// API stuff
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseSwagger();
app.UseSwaggerUI();

app.UseCors(x => x
    .AllowAnyMethod()
    .AllowAnyHeader()
    .SetIsOriginAllowed(origin => true) // allow any origin
    .AllowCredentials()); // allow credentials

// app.UseHttpsRedirection();
// app.UseAuthorization();

app.MapControllers();
app.MapHealthChecks("/");

app.Run();