using Domain.Events;
using Domain.Models;
using KwetterAPI.Models;
using MassTransit;
using Serilog;

namespace KwetterAPI.Services;

public interface IKweeterService
{
    Task<bool> PostKweet(string user, Guid userId, NewKweetDto kweetDto);
    Task<IList<Kweet>> GetKweetsByTag(string tag);
    Task<IList<Kweet>> GetKweetsByUsername(string username);
    Task<IList<Kweet>> GetRecentKweets();
    Task<Kweet> GetKweet(Guid id);
}

public class KweeterService : IKweeterService
{
    private readonly HttpClient _httpClient;
    private readonly IPublishEndpoint _publishEndpoint;

    public KweeterService(IPublishEndpoint publishEndpoint, HttpClient httpClient)
    {
        _publishEndpoint = publishEndpoint;
        _httpClient = httpClient;
    }

    public async Task<bool> PostKweet(string user, Guid userId, NewKweetDto kweetDto)
    {
        await _publishEndpoint.Publish(new NewKweet
        {
            CorrelationId = Guid.NewGuid(),
            KweetText = kweetDto.Message,
            UserName = user,
            UserId = userId
        });

        return true;
    }

    public async Task<Kweet> GetKweet(Guid id)
    {
        var response = await _httpClient.GetAsync($"by-id?id={id}");
        response.EnsureSuccessStatusCode();

        return response.Content.ReadAsAsync<Kweet>().Result;
    }

    public async Task<IList<Kweet>> GetRecentKweets()
    {
        var response = await _httpClient.GetAsync("recent");
        response.EnsureSuccessStatusCode();

        return response.Content.ReadAsAsync<IList<Kweet>>().Result;
    }

    public async Task<IList<Kweet>> GetKweetsByTag(string tag)
    {
        var response = await _httpClient.GetAsync($"for-tag?tag={tag}");
        response.EnsureSuccessStatusCode();

        return response.Content.ReadAsAsync<IList<Kweet>>().Result;
    }

    public async Task<IList<Kweet>> GetKweetsByUsername(string username)
    {
        Log.Logger.Error("Sending kweet to {Base} with endpoint {Endpoint}", _httpClient.BaseAddress,
            $"by-user?username={username}");
        var response = await _httpClient.GetAsync($"by-user?username={username}");
        response.EnsureSuccessStatusCode();

        return response.Content.ReadAsAsync<IList<Kweet>>().Result;
    }
}