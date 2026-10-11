using MediFormManager.Application.Interfaces.Repositories;
using MediFormManager.Domain.Entities.Clinical;
using MediFormManager.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace MediFormManager.Infrastructure.Repositories;

public class ComponentDataMappingRepository
    : IComponentDataMappingRepository
{
    private readonly MediFormDbContext _context;

    public ComponentDataMappingRepository(MediFormDbContext context)
    {
        _context = context;
    }

    public async Task<IReadOnlyList<ComponentDataMapping>> GetByFormVersionIdAsync(
        Guid formVersionId,
        CancellationToken cancellationToken = default)
    {
        return await _context.ComponentDataMappings
            .AsNoTracking()
            .Include(mapping => mapping.DataDictionary)
            .Where(mapping =>
                !mapping.IsDeleted &&
                !mapping.FormComponent!.IsDeleted &&
                mapping.FormComponent.FormVersionId == formVersionId &&
                !mapping.DataDictionary!.IsDeleted)
            .ToListAsync(cancellationToken);
    }
}