namespace Neo4j.Data;

public class Neo4jSettings
{
    public Neo4jSettings(string uri, string username, string password)
    {
        Uri = uri;
        Username = username;
        Password = password;
    }

    public Neo4jSettings()
    {
    }

    public string Uri { get; set; }
    public string Username { get; set; }
    public string Password { get; set; }
}