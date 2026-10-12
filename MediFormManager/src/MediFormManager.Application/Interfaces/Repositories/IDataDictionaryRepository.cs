using MediFormManager.Domain.Entities.Clinical;

namespace MediFormManager.Application.Interfaces.Repositories;

public interface IDataDictionaryRepository
{
    Task<IReadOnlyList<DataDictionary>> GetActiveAsync(CancellationToken cancellationToken = default);

    Task<bool> AreAllActiveAsync(IReadOnlyCollection<Guid> ids, CancellationToken cancellationToken = default);
}