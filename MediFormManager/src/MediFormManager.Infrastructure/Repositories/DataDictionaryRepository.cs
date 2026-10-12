using MediFormManager.Application.Interfaces.Repositories;
using MediFormManager.Domain.Entities.Clinical;
using MediFormManager.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace MediFormManager.Infrastructure.Repositories;

public class DataDictionaryRepository : IDataDictionaryRepository
{
    private readonly MediFormDbContext _context;

    public DataDictionaryRepository(MediFormDbContext context)
    {
        _context = context;
    }

    public async Task<IReadOnlyList<DataDictionary>> GetActiveAsync(CancellationToken cancellationToken = default)
    {
        return await _context.DataDictionaries
            .AsNoTracking()
            .Where(d => !d.IsDeleted)
            .OrderBy(d => d.DataKey)
            .ToListAsync(cancellationToken);
    }

    public async Task<bool> AreAllActiveAsync(IReadOnlyCollection<Guid> ids, CancellationToken cancellationToken = default)
    {
        var uniqueIds = ids.Distinct().ToArray();

        if (uniqueIds.Length == 0)
            return true;

        var count = await _context.DataDictionaries
            .AsNoTracking()
            .CountAsync(
                d => uniqueIds.Contains(d.Id) && !d.IsDeleted,
                cancellationToken);

        return count == uniqueIds.Length;
    }
}