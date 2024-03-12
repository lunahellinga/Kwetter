using Neo4j.Driver;
using Neo4j.Driver.Extensions;
using Neo4j.Models;

namespace Neo4j.Services;

public class KweetService
{
    private readonly IDriver _driver;

    public KweetService(IDriver driver)
    {
        _driver = driver;
    }

    public async Task<List<Kweet>> GetAsync()
    {
        const string query = "MATCH (k:Kweet) RETURN k ORDER BY k.timestamp DESC LIMIT 100";
        var kweets = new List<Kweet>();

        await using (var session = _driver.AsyncSession())
        {
            var result = await session.RunAsync(query);

            await result.ForEachAsync(record =>
            {
                var kweetNode = record["k"].As<INode>();
                kweets.Add(new Kweet
                {
                    Id = kweetNode["id"].As<string>(),
                    User = kweetNode["user"].As<string>(),
                    Message = kweetNode["message"].As<string>(),
                    Timestamp = DateTime.UtcNow
                });
            });
        }

        return kweets;
    }

    public async Task<Kweet?> GetAsync(string id)
    {
        const string query = "MATCH (k:Kweet) WHERE k.id = $id RETURN k";
        Kweet? kweet = null;

        using (var session = _driver.AsyncSession())
        {
            var result = await session.RunAsync(query, new { id });

            var record = await result.SingleAsync();
            if (record == null) return kweet;
            var kweetNode = record["k"].As<INode>();
            kweet = new Kweet
            {
                Id = kweetNode["id"].As<string>(),
                User = kweetNode["user"].As<string>(),
                Message = kweetNode["message"].As<string>(),
                Timestamp = DateTime.UtcNow
            };
        }

        return kweet;
    }

    public async Task CreateAsync(Kweet kweet)
    {
        const string query = "CREATE (k:Kweet {id: $id, user: $user, message: $message, timestamp: $timestamp})";
        var parameters = new
        {
            id = kweet.Id,
            user = kweet.User,
            message = kweet.Message,
            timestamp = kweet.Timestamp
        };
        using (var session = _driver.AsyncSession())
        {
            await session.RunAsync(query, parameters);
        }
    }

    public async Task UpdateAsync(Kweet kweet)
    {
        const string query = "MATCH (k:Kweet {id: $id}) SET k.message = $message, k.timestamp = $timestamp";
        var parameters = new
        {
            id = kweet.Id,
            message = kweet.Message,
            timestamp = kweet.Timestamp
        };

        using (var session = _driver.AsyncSession())
        {
            await session.RunAsync(query, parameters);
        }
    }

    public async Task RemoveAsync(string id)
    {
        const string query = "MATCH (k:Kweet) WHERE k.id = $id DELETE k";
        var parameters = new { id };

        using (var session = _driver.AsyncSession())
        {
            await session.RunAsync(query, parameters);
        }
    }
}