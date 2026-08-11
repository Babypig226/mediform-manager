using MediFormManager.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace MediFormManager.Application.Interfaces.Repositories
{
    public interface IUserRepository
    {
        Task<IEnumerable<User>> GetAllAsync();

        Task<User?> GetByIdAsync(Guid id);

        Task<User?> GetByLoginIdAsync(string loginId);

        Task AddAsync(User user);

        Task UpdateAsync(User user);       

        Task<bool> ExistsAsync(string loginId);
    }
}
