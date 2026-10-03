using MediFormManager.Domain.Entities.Forms;

namespace MediFormManager.Application.Interfaces.Repositories
{
    public interface IFormRepository
    {
        Task<IEnumerable<Form>> GetAllAsync();
        Task<Form?> GetByIdAsync(Guid id);
        Task<bool> ExistsByCodeAsync(string formCode);
        Task AddAsync(Form form);
        Task UpdateAsync(Form form);
    }
}
