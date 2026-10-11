using MediFormManager.Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MediFormManager.Api.Controllers;

[ApiController]
[Authorize]
[Route("api/clinical-data")]
public class ClinicalDataController : ControllerBase
{
    private readonly ClinicalDataService _clinicalDataService;

    public ClinicalDataController(ClinicalDataService clinicalDataService)
    {
        _clinicalDataService = clinicalDataService;
    }

    [HttpGet("form-versions/{formVersionId:guid}/patients/{patientId}/mapped-values")]
    public async Task<ActionResult<IReadOnlyDictionary<Guid, string>>> GetMappedValues(
        Guid formVersionId,
        string patientId,
        CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(patientId))
        {
            return BadRequest("PatientId is required.");
        }

        var values = await _clinicalDataService.GetMappedValuesAsync(
            formVersionId,
            patientId,
            cancellationToken);

        return Ok(values);
    }
}