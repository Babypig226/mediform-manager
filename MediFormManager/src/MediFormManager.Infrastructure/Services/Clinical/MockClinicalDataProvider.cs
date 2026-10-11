using MediFormManager.Application.Interfaces.Services;

namespace MediFormManager.Infrastructure.Services.Clinical;

public class MockClinicalDataProvider : IClinicalDataProvider
{
    public Task<IReadOnlyDictionary<string, string>> GetPatientDataAsync(
       string patientId,
       CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();

        if (patientId != "P001")
        {
            throw new KeyNotFoundException(
                $"Mock patient '{patientId}' was not found.");
        }

        IReadOnlyDictionary<string, string> data =
            new Dictionary<string, string>
            {
                ["Patient.Id"] = "P001",
                ["Patient.Name"] = "Kim Minji",
                ["Patient.BirthDate"] = "1990-05-14",
                ["Doctor.Name"] = "Dr. Lee",
                ["Encounter.Diagnosis"] = "Appendicitis"
            };

        return Task.FromResult(data);
    }
}