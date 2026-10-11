using MediFormManager.Application.Interfaces.Repositories;
using MediFormManager.Application.Interfaces.Services;

namespace MediFormManager.Application.Services;

public class ClinicalDataService
{
    private readonly IComponentDataMappingRepository _mappingRepository;
    private readonly IClinicalDataProvider _clinicalDataProvider;

    public ClinicalDataService(
        IComponentDataMappingRepository mappingRepository,
        IClinicalDataProvider clinicalDataProvider)
    {
        _mappingRepository = mappingRepository;
        _clinicalDataProvider = clinicalDataProvider;
    }

    public async Task<IReadOnlyDictionary<Guid, string>> GetMappedValuesAsync(
        Guid formVersionId,
        string patientId,
        CancellationToken cancellationToken = default)
    {
        if (formVersionId == Guid.Empty)
        {
            throw new ArgumentException(
                "FormVersionId is required.",
                nameof(formVersionId));
        }

        if (string.IsNullOrWhiteSpace(patientId))
        {
            throw new ArgumentException(
                "PatientId is required.",
                nameof(patientId));
        }

        var mappings = await _mappingRepository.GetByFormVersionIdAsync(
            formVersionId,
            cancellationToken);

        var patientData = await _clinicalDataProvider.GetPatientDataAsync(
            patientId,
            cancellationToken);

        var result = new Dictionary<Guid, string>();

        foreach (var mapping in mappings)
        {
            var dataKey = mapping.DataDictionary?.DataKey;

            if (string.IsNullOrWhiteSpace(dataKey))
            {
                continue;
            }

            if (patientData.TryGetValue(dataKey, out var value))
            {
                result[mapping.FormComponentId] = value;
            }
        }

        return result;
    }
}