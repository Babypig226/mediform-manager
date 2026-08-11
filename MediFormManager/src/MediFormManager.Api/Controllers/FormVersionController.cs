using MediFormManager.Application.DTOs.FormVersions;
using MediFormManager.Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MediFormManager.Api.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class FormVersionsController : ControllerBase
{
    private readonly FormVersionService _formVersionService;

    public FormVersionsController(FormVersionService formVersionService)
    {
        _formVersionService = formVersionService;
    }

    [HttpGet("form/{formId:guid}")]
    public async Task<ActionResult<IEnumerable<FormVersionDto>>> GetByFormId(Guid formId)
    {
        var versions = await _formVersionService.GetByFormIdAsync(formId);
        return Ok(versions);
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<FormVersionDto>> GetById(Guid id)
    {
        var version = await _formVersionService.GetByIdAsync(id);

        if (version is null)
            return NotFound();

        return Ok(version);
    }

    [HttpPost]
    public async Task<ActionResult<FormVersionDto>> Create(CreateFormVersionRequest request)
    {
        var version = await _formVersionService.CreateAsync(request);

        return CreatedAtAction(
            nameof(GetById),
            new { id = version.Id },
            version);
    }

    [HttpPut("{id:guid}")]
    public async Task<ActionResult<FormVersionDto>> Update(
        Guid id,
        UpdateFormVersionRequest request)
    {
        var version = await _formVersionService.UpdateAsync(id, request);

        if (version is null)
            return NotFound();

        return Ok(version);
    }
}