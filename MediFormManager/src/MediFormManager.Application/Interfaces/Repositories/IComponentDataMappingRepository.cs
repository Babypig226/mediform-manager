using MediFormManager.Domain.Entities.Clinical;

namespace MediFormManager.Application.Interfaces.Repositories;

public interface IComponentDataMappingRepository
{
    Task<IReadOnlyList<ComponentDataMapping>> GetByFormVersionIdAsync(
        Guid formVersionId,
        CancellationToken cancellationToken = default);
}