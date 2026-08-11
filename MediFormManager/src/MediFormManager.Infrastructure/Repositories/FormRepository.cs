using MediFormManager.Application.Interfaces.Repositories;
using MediFormManager.Domain.Entities;
using MediFormManager.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace MediFormManager.Infrastructure.Repositories
{
    public class FormRepository : IFormRepository
    {
        private readonly MediFormDbContext _context;

        public FormRepository(MediFormDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Form>> GetAllAsync()
        {
            return await _context.Forms
                .Include(f => f.Category)
                .Where(f => !f.IsDeleted)
                .ToListAsync();
        }

        public async Task<Form?> GetByIdAsync(Guid id)
        {
            return await _context.Forms
                .Include(f => f.Category)
                .Include(f => f.Versions)
                .FirstOrDefaultAsync(f => f.Id == id && !f.IsDeleted);
        }

        public async Task<bool> ExistsByCodeAsync(string formCode)
        {
            return await _context.Forms
                .AnyAsync(f => f.FormCode == formCode && !f.IsDeleted);
        }

        public async Task AddAsync(Form form)
        {
            await _context.Forms.AddAsync(form);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Form form)
        {
            _context.Forms.Update(form);
            await _context.SaveChangesAsync();
        }
    }
}
