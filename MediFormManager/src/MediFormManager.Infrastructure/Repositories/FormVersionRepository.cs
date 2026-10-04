using MediFormManager.Application.Interfaces.Repositories;
using MediFormManager.Domain.Entities.Forms;
using MediFormManager.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace MediFormManager.Infrastructure.Repositories
{
    public class FormVersionRepository : IFormVersionRepository
    {
        private readonly MediFormDbContext _context;

        public FormVersionRepository(MediFormDbContext context) {
            _context = context;
        }

        public async Task<IEnumerable<FormVersion>> GetByFormIdAsync(Guid formId) {
            return await _context.FormVersions
                    .Where(v => v.FormId == formId && !v.IsDeleted)
                    .OrderByDescending(v => v.Version)
                    .ToListAsync();
        }

        public async Task<FormVersion?> GetActiveByFormIdAsync(Guid formId)
        {
            return await _context.FormVersions
                .Where(v => v.FormId == formId && !v.IsDeleted && v.Status == "Active")
                .FirstOrDefaultAsync();                
        }

        public async Task<FormVersion?> GetByIdAsync(Guid id)
        {
            return await _context.FormVersions
                .Include(v => v.Components)
                .FirstOrDefaultAsync(v => v.Id == id && !v.IsDeleted);
        }

        public async Task<FormVersion?> GetSchemaByIdAsync(Guid id)
        {
            return await _context.FormVersions
                .Include(v => v.Components)
                    .ThenInclude(c => c.Options)
                .Include(v => v.Rules)
                    .ThenInclude(r => r.Conditions)
                .Include(v => v.Rules)
                    .ThenInclude(r => r.Actions)
                    .FirstOrDefaultAsync(v => v.Id == id && !v.IsDeleted);
        }

        public async Task<bool> ExistsVersionAsync(Guid formId, int version) {

            return await _context.FormVersions
                .AnyAsync(v =>
                v.FormId == formId &&
                v.Version == version &&
                !v.IsDeleted);
        }

        public async Task AddAsync(FormVersion formVersion)
        {
            await _context.FormVersions.AddAsync(formVersion);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(FormVersion formVersion)
        {
            _context.FormVersions.Update(formVersion);
            await _context.SaveChangesAsync();
        }

        public async Task SaveActivationAsync(FormVersion targetVersion, FormVersion? currentActiveVersion)
        {
            
                if (currentActiveVersion != null)
                {
                    _context.FormVersions.Update(currentActiveVersion);
                }
                _context.FormVersions.Update(targetVersion);
                await _context.SaveChangesAsync();          
            
        }

     

    }
}
