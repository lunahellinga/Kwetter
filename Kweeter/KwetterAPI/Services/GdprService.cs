using Keycloak.AuthServices.Authentication;
using Keycloak.AuthServices.Common;
using Newtonsoft.Json.Linq;
using Serilog;

namespace KwetterAPI.Services;

public class GdprService
{
    private readonly IConfiguration _configuration;
    private readonly HttpClient _httpClient;

    public GdprService(HttpClient httpClient, IConfiguration configuration)
    {
        _httpClient = httpClient;
        _configuration = configuration;
    }

    /// <summary>
    ///     Disable the user's account on the keycloak Kwetter realm.
    /// </summary>
    /// <param name="userId"></param>
    public async Task<bool> Delete(Guid userId)
    {
        try
        {
            Log.Logger.Information("Starting kweet anonymization");
            (await _httpClient.DeleteAsync("gdpr")).EnsureSuccessStatusCode();
            Log.Logger.Information("Kweet anonymization succeeded");
            Log.Logger.Information("Starting account deletion for user {UserId}", userId);
            var options = GetKeycloakConfig(_configuration);
            (await DeleteUser(userId, options)).EnsureSuccessStatusCode();
            Log.Logger.Information("Account deletion succeeded");

            return true;
        }
        catch (Exception e)
        {
            Log.Logger.Error("Exception during account disable {E}", e);
            return false;
        }
    }

    private static async Task<HttpResponseMessage> DeleteUser(Guid userId, KeycloakAuthenticationOptions options)
    {
        var client = new HttpClient();
        client.BaseAddress = new Uri(options.AuthServerUrl);

        var token = GetKeycloakToken(options, client);
        // var request = new HttpRequestMessage(HttpMethod.Delete, $"/auth/admin/realms/{options.Realm}/users/{userId}");
        var request = new HttpRequestMessage(HttpMethod.Delete, $"/admin/realms/{options.Realm}/users/{userId}");
        request.Headers.Add("Authorization", $"Bearer {token}");
        var response = await client.SendAsync(request);
        return response;
    }

    private static string GetKeycloakToken(KeycloakInstallationOptions keycloakAuthenticationOptions,
        HttpClient httpClient)
    {
        var request = new HttpRequestMessage(HttpMethod.Post,
            $"/realms/{keycloakAuthenticationOptions.Realm}/protocol/openid-connect/token");
        // request.Headers.Conten("Content-Type", "application/x-www-form-urlencoded");
        request.Content = new FormUrlEncodedContent(new Dictionary<string, string>
        {
            { "scope", "openid" },
            { "client_id", Convert.ToString(keycloakAuthenticationOptions.Resource) },
            { "grant_type", "client_credentials" },
            { "client_secret", Convert.ToString(keycloakAuthenticationOptions.Credentials.Secret) }
        });

        var response = httpClient.Send(request);
        response.EnsureSuccessStatusCode();
        return JObject.Parse(response.Content.ReadAsStringAsync().Result).GetValue("access_token")!.ToString();
    }


    private static KeycloakAuthenticationOptions GetKeycloakConfig(IConfiguration configuration)
    {
        var options = new KeycloakAuthenticationOptions();
        configuration
            .GetSection(KeycloakAuthenticationOptions.Section)
            .Bind(options, opt => opt.BindNonPublicProperties = true);
        return options;
    }
}