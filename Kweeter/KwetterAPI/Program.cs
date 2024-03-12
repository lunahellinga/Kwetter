using System.Security.Claims;
using System.Text.Json.Serialization;
using KwetterAPI.Extensions;
using KwetterAPI.Helpers;
using KwetterAPI.Models;
using KwetterAPI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Logging;
using Serilog;
using static KwetterAPI.Extensions.AuthorizationConstants.Policies;
using JsonOptions = Microsoft.AspNetCore.Http.Json.JsonOptions;

var builder = WebApplication.CreateBuilder(args);

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.AddSerilog();
builder.Services.AddHealthChecks();
builder.Configuration.AddEnvironmentVariables().Build();

builder.Services
    .AddApplicationSwagger(builder.Configuration)
    .AddAuth(builder.Configuration);
builder.Services.Configure<JsonOptions>(opts =>
{
    opts.SerializerOptions.ReferenceHandler = ReferenceHandler.Preserve;
    opts.SerializerOptions.WriteIndented = true;
});
builder.Services.AddCors();

var cacheUri = Environment.GetEnvironmentVariable("CACHE_URI");
builder.Services.AddHttpClient<KweeterService>(c =>
    c.BaseAddress = new Uri(cacheUri ?? throw new InvalidOperationException()));
builder.Services.AddHttpClient<GdprService>(c =>
    c.BaseAddress = new Uri(cacheUri ?? throw new InvalidOperationException()));
// Add Rabbbitmq and MassTransit
ConfigurationHelper.AddRabbitMq(builder);

// Build App
var app = builder.Build();
// Debugging
if (Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Development")
    IdentityModelEventSource.ShowPII = true;

app.UseHttpsRedirection();
app.UseApplicationSwagger(builder.Configuration);
app.UseAuthentication();
app.UseAuthorization();
app.UseCors(x => x
    .AllowAnyMethod()
    .AllowAnyHeader()
    .SetIsOriginAllowed(origin => true) // allow any origin
    .AllowCredentials()); // allow credentials

// Endpoints
app.MapPost("/post-kweet", async (
            [FromBody] NewKweetDto kweet, KweeterService kweeterService, ClaimsPrincipal user) =>
        await kweeterService.PostKweet(
            ClaimHelper.GetUserName(user),
            ClaimHelper.GetUserId(user),
            kweet)
            ? Results.Ok()
            : Results.BadRequest())
    .RequireAuthorization(RequireRealmRole);

app.MapGet("/by-id", async (Guid id, KweeterService kweeterService) =>
        await kweeterService.GetKweet(id) is { } kweet
            ? Results.Ok(kweet)
            : Results.NotFound())
    .RequireAuthorization(RequireRealmRole);

app.MapGet("/by-tag", async (string tag, KweeterService kweeterService) =>
        await kweeterService.GetKweetsByTag(tag) is { } kweets
            ? Results.Ok(kweets)
            : Results.NotFound())
    .RequireAuthorization(RequireRealmRole);

app.MapGet("/by-user", async (string username, KweeterService kweeterService) =>
        await kweeterService.GetKweetsByUsername(username) is { } kweets
            ? Results.Ok(kweets)
            : Results.NotFound())
    .RequireAuthorization(RequireRealmRole);

app.MapGet("/recent", async (KweeterService kweeterService) =>
        await kweeterService.GetRecentKweets() is { } kweets
            ? Results.Ok(kweets)
            : Results.NotFound())
    .RequireAuthorization(RequireRealmRole);

app.MapDelete("/gdpr-delete", async (GdprService gdprService, ClaimsPrincipal user) =>
    await gdprService.Delete(ClaimHelper.GetUserId(user))
    // await gdprService.Delete(new Guid("0fc24b9f-88b5-454b-9577-ef99f5259155"))
        ? Results.Ok()
        : Results.BadRequest())
        // : Results.BadRequest());
    .RequireAuthorization(RequireRealmRole);

app.MapHealthChecks("/");
app.UseSerilogRequestLogging();

app.Run();