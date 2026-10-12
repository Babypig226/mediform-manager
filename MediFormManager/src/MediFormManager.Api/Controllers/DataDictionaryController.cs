using MediFormManager.Application.DTOs.Clinical;
using MediFormManager.Application.Interfaces.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MediFormManager.Api.Controllers;

[ApiController]
[Authorize]
[Route("api/data-dictionary")]
public class DataDictionaryController : ControllerBase
{
    private readonly IDataDictionaryRepository _repository;

    public DataDictionaryController(IDataDictionaryRepository repository)
    {
        _repository = repository;
    }

    [HttpGet]
    public async Task<ActionResult<List<DataDictionaryDto>>> GetAll(
        CancellationToken cancellationToken)
    {
        var items = await _repository.GetActiveAsync(cancellationToken);

        return Ok(items.Select(d => new DataDictionaryDto
        {
            Id = d.Id,
            DataKey = d.DataKey,
            DisplayName = d.DisplayName,
            DataType = d.DataType
        }).ToList());
    }
}