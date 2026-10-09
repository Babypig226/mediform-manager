using MediFormManager.Application.DTOs.FormSchemas;
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

    [HttpGet("{id:guid}/schema")]
    public async Task<ActionResult<FormSchemaDto>> GetSchema(Guid id) {
        var schema = await _formVersionService.GetSchemaByIdAsync(id);
        return Ok(schema);
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

    [HttpPost("{id:guid}/activate")]
    public async Task<IActionResult> Activate(Guid id, [FromQuery]bool replaceExisting = false)
    {
        await _formVersionService.ActivateAsync(id, replaceExisting);
        return Ok();
    }

    [HttpPut("{id:guid}/schema")]
    public async Task<IActionResult> SaveSchema(Guid id, [FromBody] SaveFormSchemaRequest request)
    {
        await _formVersionService.SaveSchemaAsync(id, request);

        return NoContent();
    }
}