using MediFormManager.Domain.Entities.Components;
using MediFormManager.Domain.Entities.Forms;

namespace MediFormManager.Application.Interfaces.Repositories
{
    public interface IFormVersionRepository
    {
        Task<IEnumerable<FormVersion>> GetByFormIdAsync(Guid formId);
        Task<FormVersion?> GetByIdAsync(Guid id);
        Task<FormVersion?> GetActiveByFormIdAsync(Guid id);

        Task<FormVersion?> GetSchemaByIdAsync(Guid id);

        Task<bool> ExistsVersionAsync(Guid formId, int version);
        
        Task AddAsync(FormVersion formVersion);
        Task UpdateAsync(FormVersion formVersion);

        Task SaveActivationAsync(FormVersion targetVersion, FormVersion? currentActiveVersion);

        Task SaveSchemaChangesAsync();

        Task<bool> AnyComponentIdsExistAsync(IEnumerable<Guid> componentIds);

        Task<bool> AnyOptionIdsExistAsync(IEnumerable<Guid> optionIds);

        void AddComponent(FormComponent component);
    }
}
