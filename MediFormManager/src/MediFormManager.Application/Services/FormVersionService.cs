using MediFormManager.Application.DTOs.Forms;
using MediFormManager.Application.DTOs.FormVersions;
using MediFormManager.Application.Interfaces.Repositories;
using MediFormManager.Domain.Entities.Forms;
using MediFormManager.Application.Exceptions;
using MediFormManager.Application.DTOs.FormSchemas;

namespace MediFormManager.Application.Services
{
    public class FormVersionService
    {
        private readonly IFormVersionRepository _formVersionRepository;
        private readonly IFormRepository _formRepository;

        public FormVersionService(IFormVersionRepository formVersionRepository, IFormRepository formRepository)
        {
            _formVersionRepository = formVersionRepository;
            _formRepository = formRepository;
        }

        public async Task<IEnumerable<FormVersionDto>> GetByFormIdAsync(Guid formId)
        {
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

        public async Task<FormVersionDto?> GetByIdAsync(Guid id)
        {
            var version = await _formVersionRepository.GetByIdAsync(id);

            if(version is null)
            {
                throw new NotFoundException($"FormVersion with Id '{id}' not found.");
            }

            return new FormVersionDto
            {
                Id = version.Id,
                FormId = version.FormId,
                Version = version.Version,
                Status = version.Status,
                CreatedAt = version.CreatedAt
            };
        }

        public async Task<FormSchemaDto> GetSchemaByIdAsync(Guid id)
        {
            var version = await _formVersionRepository.GetSchemaByIdAsync(id);
            if (version is null)
            {
                throw new NotFoundException($"FormVersion with Id '{id}' not found.");
            }
            return new FormSchemaDto
            {
                FormVersionId = version.Id,

                Version = version.Version,
                Status = version.Status,

                Components = version.Components
                .OrderBy(c => c.Order)
                .Select(c => new FormComponentDto
                {
                    Id = c.Id,
                    ComponentType = c.ComponentType,
                    Label = c.Label,
                    Prompt = c.Prompt,
                    IsRequired = c.IsRequired,
                    IsDisabled = c.IsDisabled,
                    IsVisible = c.IsVisible,
                    DefaultValue = c.DefaultValue,
                    GroupKey = c.GroupKey,
                    Placeholder = c.Placeholder,
                    Order = c.Order,
                    Options = c.Options
                        .OrderBy(o => o.Order)
                        .Select(o => new ComponentOptionDto
                        {
                            Id = o.Id,
                            DisplayText = o.DisplayText,
                            Order = o.Order
                        }).ToList()
                }).ToList(),
                Rules = version.Rules.Select(r => new ComponentRuleDto
                {
                    Id = r.Id,
                    RuleName = r.RuleName,
                    Logic = r.Logic,
                    Conditions = r.Conditions.Select(cond => new RuleConditionDto
                    {
                        Id = cond.Id,
                        SourceComponentId = cond.SourceComponentId,
                        Operator = cond.Operator,
                        ExpectedOptionId = cond.ExpectedOptionId,
                        ExpectedValue = cond.ExpectedValue
                    }).ToList(),
                    Actions = r.Actions.Select(act => new RuleActionDto
                    {
                        Id = act.Id,
                        ActionType = act.ActionType,
                        TargetType = act.TargetType,
                        TargetComponentId = act.TargetComponentId,
                        TargetGroupKey = act.TargetGroupKey
                    }).ToList()
                }).ToList()
            };
        }

        public async Task<FormVersionDto> CreateAsync(CreateFormVersionRequest request)
        {
            var form = await _formRepository.GetByIdAsync(request.FormId);

            if (form is null)
            {
                throw new NotFoundException($"Form with Id '{request.FormId}' not found.");
            }

            if (await _formVersionRepository.ExistsVersionAsync(request.FormId, request.Version))
            {
                throw new ConflictException($"Version {request.Version} already exists for this form.");
            }

            var formVersion = new FormVersion
            {
                Id = Guid.NewGuid(),
                FormId = request.FormId,
                Version = request.Version,
                Status = "Draft"
            };

            await _formVersionRepository.AddAsync(formVersion);

            return new FormVersionDto
            {
                Id = formVersion.Id,
                FormId = formVersion.FormId,
                Version = formVersion.Version,
                Status = formVersion.Status,
                CreatedAt = formVersion.CreatedAt
            };

        }

        public async Task<FormVersionDto?> UpdateAsync(Guid id, UpdateFormVersionRequest request)
        {
            var formVersion = await _formVersionRepository.GetByIdAsync(id);
            if (formVersion is null)
            {
                return null;
            }

            var allowedStatuses = new[] { "Draft", "Active", "Archived" };

            if (!allowedStatuses.Contains(request.Status))
            {
                throw new ValidationException($"Invalid status '{request.Status}'.");
            }

            formVersion.Status = request.Status;
            formVersion.UpdatedAt = DateTime.UtcNow;

            await _formVersionRepository.UpdateAsync(formVersion);

            var updatedFormVersion = await _formVersionRepository.GetByIdAsync(id);

            if (updatedFormVersion is null)
            {
                throw new NotFoundException($"FormVersion with Id '{id}' not found.");
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

        public async Task ActivateAsync(Guid versionId, bool replaceExisting = false)
        {
            var formVersion = await _formVersionRepository.GetByIdAsync(versionId);
            if (formVersion is null)
            {
                throw new NotFoundException($"FormVersion with Id '{versionId}' not found.");
            }

            if (formVersion.Status == "Active")
            {
                throw new ConflictException($"FormVersion with Id '{versionId}' is already active.");
            }

            var currentActiveVersion = await _formVersionRepository.GetActiveByFormIdAsync(formVersion.FormId);

            if (currentActiveVersion is not null && !replaceExisting)
            {
                throw new ConflictException($"Form '{formVersion.FormId}' already has an active version.");
            }

            var now = DateTime.UtcNow;

            if (currentActiveVersion is not null)
            {
                currentActiveVersion.Status = "Archived";
                currentActiveVersion.UpdatedAt = now;
            }

            formVersion.Status = "Active";
            formVersion.UpdatedAt = now;
            await _formVersionRepository.SaveActivationAsync(formVersion, currentActiveVersion);

        }


    }
}
