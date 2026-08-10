using MediFormManager.Application.Interfaces.Repositories;
using MediFormManager.Domain.Entities;
using MediFormManager.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace MediFormManager.Infrastructure.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly MediFormDbContext _context;

        public UserRepository(MediFormDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(User user)
        {
            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();
        }
      

        public async Task<bool> ExistsAsync(string loginId)
        {
            return await _context.Users.AnyAsync(u => u.LoginId == loginId && !u.IsDeleted);
        }

        public async Task<IEnumerable<User>> GetAllAsync()
        {
            return await _context.Users
                .Include(u => u.Role)
                .Include(u => u.Department)
                .Include(u => u.JobPosition)
                .Where(u => !u.IsDeleted)
                .ToListAsync();
        }

        public async Task<User?> GetByIdAsync(Guid id)
        {
           return await _context.Users
                .Include(u => u.Role)
                .Include(u => u.Department)
                .Include(u => u.JobPosition)
                .FirstOrDefaultAsync(u => u.Id == id && !u.IsDeleted);
        }

        public async Task<User?> GetByLoginIdAsync(string loginId)
        {
            return await _context.Users
                .Include(u => u.Role)
                .Include(u => u.Department)
                .Include(u => u.JobPosition)
                .FirstOrDefaultAsync(u => u.LoginId == loginId && !u.IsDeleted);
        }

        public async Task UpdateAsync(User user)
        {
            _context.Users.Update(user);
            await _context.SaveChangesAsync();
        }
    }
}
