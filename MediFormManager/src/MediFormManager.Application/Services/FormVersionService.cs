using MediFormManager.Application.DTOs.Forms;
using MediFormManager.Application.DTOs.FormVersions;
using MediFormManager.Application.Interfaces.Repositories;
using MediFormManager.Domain.Entities.Forms;
using MediFormManager.Application.Exceptions;
using MediFormManager.Application.DTOs.FormSchemas;
using MediFormManager.Domain.Enums;
using System.Reflection.Emit;
using System.ComponentModel;
using MediFormManager.Domain.Entities.Components;

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
                UpdatedAt = version.UpdatedAt,
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

        public async Task SaveSchemaAsync(Guid versionId, SaveFormSchemaRequest request) {
            var version = await _formVersionRepository.GetSchemaByIdAsync(versionId);

            if (version is null) {
                throw new NotFoundException($"FormVersion with Id '{versionId}' not found.");
            }

            if (version.Status != "Draft") {
                throw new ConflictException("Only Draft form versions can be edited");
            }

            ValidateSchemaRequest(request);

            ValidateRuleReferences(version, request);

            await ValidateComponentOwnershipAsync(version, request);
            System.Diagnostics.Debug.WriteLine(
    $"DB UpdatedAt      : {version.UpdatedAt:O}");

            System.Diagnostics.Debug.WriteLine(
                $"Request UpdatedAt : {request.ExpectedUpdatedAt:O}");

            System.Diagnostics.Debug.WriteLine(
                $"DB Ticks          : {version.UpdatedAt?.Ticks}");

            System.Diagnostics.Debug.WriteLine(
                $"Request Ticks     : {request.ExpectedUpdatedAt?.Ticks}");
            if (version.UpdatedAt != request.ExpectedUpdatedAt)
            {
                throw new ConflictException(
                    "This form version has been modified by another user.");
            }

            SyncComponents(version, request);

            version.UpdatedAt = DateTime.UtcNow;

            await _formVersionRepository.SaveSchemaChangesAsync();


        }

        private static void ValidateSchemaRequest(SaveFormSchemaRequest request) {
            if (request is null || request.Components is null) {
                throw new ValidationException("Schema components are required");
            }

            var componentIds = new HashSet<Guid>();
            var optionIds = new HashSet<Guid>();

            foreach (var component in request.Components) {
                if (component.Id == Guid.Empty) {
                    throw new ValidationException("Component ID cannot be empty");
                }

                if (!componentIds.Add(component.Id)) {
                    throw new ValidationException($"Duplicate Component ID: {component.Id}");
                }

                if (component.Options is null) {
                    throw new ValidationException($"Options are missing for Component {component.Id}");
                }

                foreach (var option in component.Options) {
                    if (option.Id == Guid.Empty) {
                        throw new ValidationException("Option ID cannot be empty.");
                    }

                    if (!optionIds.Add(option.Id)) {
                        throw new ValidationException($"Duplicate Option ID: {option.Id}");
                    }

                    
                
                }
            }
        }

        private void SyncComponents(FormVersion version, SaveFormSchemaRequest request) {
            var requestedComponents = request.Components;

            foreach (var existing in version.Components) {
                var stillExists = requestedComponents.Any(c => c.Id == existing.Id);

                if (!stillExists) {
                    existing.IsDeleted = true;
                    existing.UpdatedAt = DateTime.UtcNow;
                }
            }

            foreach (var requested in requestedComponents) {
                var existing = version.Components.FirstOrDefault(c => c.Id == requested.Id);

                if (existing is null)
                {

                    var newComponent = new FormComponent
                    {
                        Id = requested.Id,
                        FormVersionId = version.Id,
                        ComponentType = requested.ComponentType,
                        Label = requested.Label,
                        Prompt = requested.Prompt,
                        Placeholder = requested.Placeholder,
                        DefaultValue = requested.DefaultValue,
                        IsRequired = requested.IsRequired,
                        IsDisabled = requested.IsDisabled,
                        IsVisible = requested.IsVisible,
                        GroupKey = requested.GroupKey,
                        Order = requested.Order
                    };

                    _formVersionRepository.AddComponent(newComponent);
                    SyncOptions(newComponent, requested.Options ?? new());

                }
                else
                {
                    existing.ComponentType = requested.ComponentType;
                    existing.Label = requested.Label;
                    existing.Prompt = requested.Prompt;
                    existing.Placeholder = requested.Placeholder;
                    existing.DefaultValue = requested.DefaultValue;
                    existing.IsRequired = requested.IsRequired;
                    existing.IsDisabled = requested.IsDisabled;
                    existing.IsVisible = requested.IsVisible;
                    existing.GroupKey = requested.GroupKey;
                    existing.Order = requested.Order;
                    existing.UpdatedAt = DateTime.UtcNow;
                    SyncOptions(existing, requested.Options ?? new());
                }
            }
        }

        private static void SyncOptions(FormComponent component, List<SaveComponentOptionDto> requestedOptions) {
            var now = DateTime.UtcNow;

            foreach (var existing in component.Options) {
                if (!requestedOptions.Any(o => o.Id == existing.Id))
                {
                    existing.IsDeleted = true;
                    existing.UpdatedAt = now;
                }
            }

            foreach (var requested in requestedOptions) {
                var existing = component.Options.FirstOrDefault(o => o.Id == requested.Id);

                if (existing is null)
                {
                    var newOption = new ComponentOption
                    {
                        Id = requested.Id,
                        FormComponentId = component.Id,
                        DisplayText = requested.DisplayText ?? "",
                        Order = requested.Order
                    };

                    component.Options.Add(newOption);
                }
                else
                {
                    existing.DisplayText = requested.DisplayText ?? "";
                    existing.Order = requested.Order;
                    existing.UpdatedAt = now;
                }
            }
        }

        private static void ValidateRuleReferences(FormVersion version, SaveFormSchemaRequest request) {
            var componentIds = request.Components.Select(c => c.Id)
                                      .ToHashSet();

            var optionIds = request.Components.SelectMany(c => c.Options ?? new())
                                              .Select(o => o.Id)
                                              .ToHashSet();

            foreach (var rule in version.Rules) {
                foreach (var condition in rule.Conditions) {
                    if (!componentIds.Contains(condition.SourceComponentId)) {
                        throw new ConflictException($"Rule '{rule.RuleName}' references a removed source components.");
                    }

                    if (condition.ExpectedOptionId.HasValue && !optionIds.Contains(condition.ExpectedOptionId.Value)) {
                        throw new ConflictException($"Rule '{rule.RuleName}' references a removed option.");
                    }

                    foreach (var action in rule.Actions)
                    {
                        if (
                            action.TargetComponentId.HasValue &&
                            !componentIds.Contains(action.TargetComponentId.Value)
                        )
                        {
                            throw new ConflictException(
                                $"Rule '{rule.RuleName}' references a removed target component.");
                        }
                    }
                }
            }
        }

        private async Task ValidateComponentOwnershipAsync(FormVersion version, SaveFormSchemaRequest request)
        {
            var existingComponentIds = version.Components
                .Select(c => c.Id)
                .ToHashSet();

            var existingOptionIds = version.Components
                .SelectMany(c => c.Options)
                .Select(o => o.Id)
                .ToHashSet();

            var newComponentIds = request.Components
                .Select(c => c.Id)
                .Where(id => !existingComponentIds.Contains(id))
                .ToArray();

            var newOptionIds = request.Components
                .SelectMany(c => c.Options ?? new())
                .Select(o => o.Id)
                .Where(id => !existingOptionIds.Contains(id))
                .ToArray();

            if (await _formVersionRepository
                .AnyComponentIdsExistAsync(newComponentIds))
            {
                throw new ConflictException(
                    "One or more new Component IDs already exist.");
            }

            if (await _formVersionRepository
                .AnyOptionIdsExistAsync(newOptionIds))
            {
                throw new ConflictException(
                    "One or more new Option IDs already exist.");
            }

            // 기존 Option이 다른 Component로 이동되는 것도 금지
            foreach (var component in request.Components)
            {
                foreach (var option in component.Options ?? new())
                {
                    var existingOwner = version.Components
                        .FirstOrDefault(c =>
                            c.Options.Any(o => o.Id == option.Id));

                    if (existingOwner is not null &&
                        existingOwner.Id != component.Id)
                    {
                        throw new ConflictException(
                            $"Option '{option.Id}' belongs to another Component.");
                    }
                }
            }
        }

    }
}
