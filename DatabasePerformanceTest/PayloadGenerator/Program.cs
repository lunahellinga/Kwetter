using Newtonsoft.Json;

namespace PayloadGenerator;

public class KweetDto
{
    public Guid Id { get; set; }
    public Guid User { get; set; }
    public string Message { get; set; }

    public KweetDto(Guid id, Guid user, string message)
    {
        Id = id;
        User = user;
        Message = message;
    }

    public KweetDto()
    {
    }
}

public class Payload
{
    public List<KweetDto> Posts;
    public List<KweetDto> Updates;
    public List<Guid> Gets;

    public Payload(List<KweetDto> posts, List<KweetDto> updates, List<Guid> gets)
    {
        Posts = posts;
        Updates = updates;
        Gets = gets;
    }
}

public class PayloadGenerator
{
    public static void Main()
    {
        var postPayloads = new List<KweetDto>();
        var updatePayloads = new List<KweetDto>();
        var getPayloads = new List<Guid>();

        Console.WriteLine("Starting...");

        // Generate multiple payloads with random field values
        for (var i = 0; i < 30_000; i++)
        {
            var postPayload = new KweetDto
            {
                Id = Guid.NewGuid(),
                User = Guid.NewGuid(),
                Message = GenerateRandomMessage()
            };
            postPayloads.Add(postPayload);

            var updatePayload = new KweetDto
            {
                Id = postPayload.Id,
                User = postPayload.User,
                Message = GenerateRandomMessage()
            };
            updatePayloads.Add(updatePayload);

            getPayloads.Add(postPayload.Id);
        }

        Console.WriteLine($"Done generating. Serializing...");
        // Serialize the payloads to JSON
        var payload = new Payload(posts: postPayloads, updates: updatePayloads, gets: getPayloads);
        var json = JsonConvert.SerializeObject(payload, Formatting.Indented);

        Console.WriteLine("Writing to file...");
        // Save the JSON to a file
        File.WriteAllText("payload.json", json);
    }

    private static string GenerateRandomMessage()
    {
        var messages = new[]
        {
            "Hello",
            "Welcome",
            "Testing",
            "K6 is awesome",
            "Load testing",
            "Performance testing"
        };

        var random = new Random();
        return messages[random.Next(messages.Length)] + " " + random.Next(1, 1000);
    }
}