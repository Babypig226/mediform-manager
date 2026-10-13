using System.Net.Http;
using System.Net.Http.Headers;

namespace MediFormManager.Wpf.Services;

public class ApiClient
{
    private readonly HttpClient _httpClient;

    public string? AccessToken { get; private set; }

    public ApiClient()
    {
        _httpClient = new HttpClient
        {
            BaseAddress = new Uri("https://localhost:7276/")
        };
    }

    public HttpClient Client => _httpClient;

    public void SetBearerToken(string token)
    {
        AccessToken = token;
        _httpClient.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue("Bearer", token);
    }

    public void ClearBearerToken()
    {
        AccessToken = null;
        _httpClient.DefaultRequestHeaders.Authorization = null;
    }
}