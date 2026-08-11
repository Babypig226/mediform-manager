using MediFormManager.Application.DTOs.Forms;
using MediFormManager.Application.DTOs.FormVersions;
using MediFormManager.Application.Interfaces.Repositories;
using MediFormManager.Domain.Entities;

namespace MediFormManager.Application.Services
{
    public class FormVersionService
    {
        private readonly IFormVersionRepository _formVersionRepository;
        private readonly IFormRepository _formRepository;

        public FormVersionService(IFormVersionRepository formVersionRepository, IFormRepository formRepository) {
            _formVersionRepository = formVersionRepository;
            _formRepository = formRepository;
        }

        public async Task<IEnumerable<FormVersionDto>> GetByFormIdAsync(Guid formId) {
            var versions = await _formVersionRepository.GetByFormIdAsync(formId);

            return versions.Select(v => new FormVersionDto
            {
                Id = v.Id,
                FormId = v.FormId,
                Version = v.Version,
                Status = v.Status,
                CreatedAt = v.CreatedAt
            });
        }

        public async Task<FormVersionDto?> GetByIdAsync(Guid id) {
            var version = await _formVersionRepository.GetByIdAsync(id);

            return new FormVersionDto
            {
                Id = version.Id,
                FormId = version.FormId,
                Version = version.Version,
                Status = version.Status,
                CreatedAt = version.CreatedAt
            };
        }

        public async Task<FormVersionDto> CreateAsync(CreateFormVersionRequest request) {
            var form = await _formRepository.GetByIdAsync(request.FormId);

            if (form is null) {
                throw new InvalidOperationException($"Form with Id '{request.FormId}' not found.");
            }

            if (await _formVersionRepository.ExistsVersionAsync(request.FormId, request.Version)) {
                throw new InvalidOperationException($"Version {request.Version} already exists for this form."); ;
            }

            var formVersion = new FormVersion
            {
                Id = Guid.NewGuid(),
                FormId = request.FormId,
                Version = request.Version,
                Status = "Draft"
            };

            await _formVersionRepository.AddAsync(formVersion);

            return new FormVersionDto { 
                Id = formVersion.Id,
                FormId = formVersion.FormId,
                Version = formVersion.Version,
                Status = formVersion.Status,
                CreatedAt = formVersion.CreatedAt
            };
        
        }

        public async Task<FormVersionDto?> UpdateAsync(Guid id, UpdateFormVersionRequest request) {
            var formVersion = await _formVersionRepository.GetByIdAsync(id);
            if (formVersion is null)
            {
                return null;
            }

            var allowedStatuses = new[] { "Draft", "Active", "Archived" };

            if (!allowedStatuses.Contains(request.Status))
            {
                throw new InvalidOperationException(
                    $"Invalid status '{request.Status}'.");
            }

            formVersion.Status = request.Status;
            formVersion.UpdatedAt = DateTime.UtcNow;

            await _formVersionRepository.UpdateAsync(formVersion);

            var updatedFormVersion = await _formVersionRepository.GetByIdAsync(id);

            if (updatedFormVersion is null)
            {
                return null;
            }

            return new FormVersionDto
            {
                Id = updatedFormVersion.Id,
                FormId = updatedFormVersion.FormId,
                Version = updatedFormVersion.Version,                
                Status = updatedFormVersion.Status,
                CreatedAt = updatedFormVersion.CreatedAt
            };

        }


    }
}
