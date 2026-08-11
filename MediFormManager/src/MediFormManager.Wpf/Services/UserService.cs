using MediFormManager.Wpf.Models;
using System.Net.Http.Json;

namespace MediFormManager.Wpf.Services;

public class UserService
{
    private readonly ApiClient _apiClient;

    public UserService(ApiClient apiClient)
    {
        _apiClient = apiClient;
    }

    public async Task<IEnumerable<UserDto>> GetAllAsync()
    {
        var users = await _apiClient.Client
            .GetFromJsonAsync<IEnumerable<UserDto>>("api/User");

        return users ?? Enumerable.Empty<UserDto>();
    }
}