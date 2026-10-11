namespace MediFormManager.Application.Interfaces.Services;

public interface IClinicalDataProvider
{
    Task<IReadOnlyDictionary<string, string>> GetPatientDataAsync(
        string patientId,
        CancellationToken cancellationToken = default);
}