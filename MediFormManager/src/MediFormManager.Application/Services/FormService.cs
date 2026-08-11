using MediFormManager.Application.DTOs.Forms;
using MediFormManager.Application.Interfaces.Repositories;
using MediFormManager.Domain.Entities;

namespace MediFormManager.Application.Services;

public class FormService
{
    private readonly IFormRepository _formRepository;

    public FormService(IFormRepository formRepository)
    {
        _formRepository = formRepository;
    }

    public async Task<IEnumerable<FormDto>> GetAllAsync()
    {
        var forms = await _formRepository.GetAllAsync();
        return forms.Select(f => new FormDto
        {
            Id = f.Id,
            FormCode = f.FormCode,
            FormName = f.FormName,
            CategoryId = f.CategoryId,
            CategoryName = f.Category?.CategoryName ?? string.Empty,
            IsActive = f.IsActive,
            CreatedAt = f.CreatedAt
        });
    }

    public async Task<FormDto?> GetByIdAsync(Guid id)
    {
        var form = await _formRepository.GetByIdAsync(id);

        if (form is null)
            return null;

        //TODO : Consider using AutoMapper for mapping entities to DTOs in the future.
        return new FormDto
        {
            Id = form.Id,
            FormCode = form.FormCode,
            FormName = form.FormName,
            CategoryId = form.CategoryId,
            CategoryName = form.Category?.CategoryName ?? string.Empty,
            IsActive = form.IsActive,
            CreatedAt = form.CreatedAt
        };
    }

    public async Task<FormDto> CreateAsync(CreateFormRequest request)
    {
        if (await _formRepository.ExistsByCodeAsync(request.FormCode))
        {
            throw new InvalidOperationException(
                $"FormCode '{request.FormCode}' already exists.");
        }

        var form = new Form
        {
            Id = Guid.NewGuid(),
            FormCode = request.FormCode,
            FormName = request.FormName,
            CategoryId = request.CategoryId,
            IsActive = true
        };

        await _formRepository.AddAsync(form);

        var createdForm = await _formRepository.GetByIdAsync(form.Id);

        if (createdForm is null)
        {
            throw new InvalidOperationException("Failed to retrieve created form.");
        }

        return new FormDto
        {
            Id = createdForm.Id,
            FormCode = createdForm.FormCode,
            FormName = createdForm.FormName,
            CategoryId = createdForm.CategoryId,
            CategoryName = createdForm.Category?.CategoryName ?? string.Empty,
            IsActive = createdForm.IsActive,
            CreatedAt = createdForm.CreatedAt
        };
    }

    public async Task<FormDto?> UpdateAsync(Guid id, UpdateFormRequest request)
    {
        var form = await _formRepository.GetByIdAsync(id);
        if (form is null)
        {
            return null;
        }
        
        form.FormName = request.FormName;
        form.CategoryId = request.CategoryId;
        form.IsActive = request.IsActive;
        form.UpdatedAt = DateTime.UtcNow;

        await _formRepository.UpdateAsync(form);

        var updatedForm = await _formRepository.GetByIdAsync(id);

        if (updatedForm is null) {
            return null;
        }

        return new FormDto { 
            Id = updatedForm.Id, 
            FormCode = updatedForm.FormCode, 
            FormName = updatedForm.FormName, 
            CategoryId = updatedForm.CategoryId, 
            CategoryName = updatedForm.Category?.CategoryName ?? string.Empty,
            IsActive = updatedForm.IsActive, 
            CreatedAt = updatedForm.CreatedAt
        };
    }
}