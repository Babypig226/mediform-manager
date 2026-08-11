using MediFormManager.Wpf.Models;
using System.Net.Http.Json;

namespace MediFormManager.Wpf.Services;

public class AuthService
{
    private readonly ApiClient _apiClient;

    public AuthService(ApiClient apiClient)
    {
        _apiClient = apiClient;
    }

    public async Task<LoginResponse?> LoginAsync(
        string loginId,
        string password)
    {
        var request = new LoginRequest
        {
            LoginId = loginId,
            Password = password
        };

        var response = await _apiClient.Client.PostAsJsonAsync(
            "api/Auth/login",
            request);

        if (!response.IsSuccessStatusCode)
            return null;

        var result = await response.Content
            .ReadFromJsonAsync<LoginResponse>();

        if (result is null)
            return null;

        _apiClient.SetBearerToken(result.AccessToken);

        return result;
    }
}