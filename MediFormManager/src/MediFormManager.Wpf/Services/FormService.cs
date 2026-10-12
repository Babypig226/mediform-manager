using MediFormManager.Wpf.Models;
using System.Net.Http.Json;

namespace MediFormManager.Wpf.Services;

public class FormService
{
    private readonly ApiClient _apiClient;

    public FormService(ApiClient apiClient)
    {
        _apiClient = apiClient;
    }

    public async Task<IEnumerable<FormDto>> GetAllAsync()
    {
        var forms = await _apiClient.Client
            .GetFromJsonAsync<IEnumerable<FormDto>>("api/Forms");

        return forms ?? Enumerable.Empty<FormDto>();
    }

    public async Task<IEnumerable<FormVersionDto>> GetVersionsAsync(Guid formId)
    {
        var versions = await _apiClient.Client
            .GetFromJsonAsync<IEnumerable<FormVersionDto>>(
                $"api/FormVersions/form/{formId}");

        return versions ?? Enumerable.Empty<FormVersionDto>();
    }
}