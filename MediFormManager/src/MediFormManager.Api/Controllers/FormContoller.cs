using MediFormManager.Application.DTOs.Forms;
using MediFormManager.Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MediFormManager.Api.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class FormsController : ControllerBase
{
    private readonly FormService _formService;

    public FormsController(FormService formService)
    {
        _formService = formService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<FormDto>>> GetAll()
    {
        var forms = await _formService.GetAllAsync();
        return Ok(forms);
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<FormDto>> GetById(Guid id)
    {
        var form = await _formService.GetByIdAsync(id);

        if (form is null)
            return NotFound();

        return Ok(form);
    }

    [HttpPost]
    public async Task<ActionResult<FormDto>> Create(CreateFormRequest request)
    {
        var form = await _formService.CreateAsync(request);

        return CreatedAtAction(
            nameof(GetById),
            new { id = form.Id },
            form);
    }

    [HttpPut("{id:guid}")]
    public async Task<ActionResult<FormDto>> Update(
        Guid id,
        UpdateFormRequest request)
    {
        var form = await _formService.UpdateAsync(id, request);

        if (form is null)
            return NotFound();

        return Ok(form);
    }
}