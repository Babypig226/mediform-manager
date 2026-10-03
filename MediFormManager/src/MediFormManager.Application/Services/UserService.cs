using MediFormManager.Application.DTOs.Users;
using MediFormManager.Application.Exceptions;
using MediFormManager.Application.Interfaces.Repositories;
using MediFormManager.Domain.Entities.Users;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;

namespace MediFormManager.Application.Services
{
    public class UserService
    {

        private readonly IUserRepository _userRepository;
        private readonly IPasswordHasher<User> _passwordHasher;

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
            _passwordHasher = new PasswordHasher<User>();
        }

        public async Task<IEnumerable<UserDto>> GetAllAsync()
        {
            var users = await _userRepository.GetAllAsync();
            return users.Select(u => new UserDto
            {
                Id = u.Id,
                LoginId = u.LoginId,
                UserName = u.UserName,
                RoleId = u.RoleId,
                RoleName = u.Role?.RoleName ?? string.Empty,
                DepartmentId = u.DepartmentId,
                DepartmentName = u.Department?.DepartmentName ?? string.Empty,
                JobPositionId = u.JobPositionId,
                JobPositionName = u.JobPosition?.JobName ?? string.Empty,
                IsActive = u.IsActive,
                CreatedAt = u.CreatedAt
            });
        }

        public async Task<UserDto?> GetByIdAsync(Guid id)
        {
            var user = await _userRepository.GetByIdAsync(id);

            if (user == null) return null;

            return new UserDto
            {
                Id = user.Id,
                LoginId = user.LoginId,
                UserName = user.UserName,
                RoleId = user.RoleId,
                RoleName = user.Role?.RoleName ?? string.Empty,
                DepartmentId = user.DepartmentId,
                DepartmentName = user.Department?.DepartmentName ?? string.Empty,
                JobPositionId = user.JobPositionId,
                JobPositionName = user.JobPosition?.JobName ?? string.Empty,
                IsActive = user.IsActive,
                CreatedAt = user.CreatedAt
            };
        }

        public async Task<UserDto> CreateAsync(CreateUserRequest request)
        {

            // Validate that the LoginId is unique
            if (await _userRepository.ExistsAsync(request.LoginId))
            {
                throw new ConflictException($"LoginId '{request.LoginId}' already exists.");
            }

            // Request to Entity
            var user = new User
            {
                Id = Guid.NewGuid(),
                LoginId = request.LoginId,
                UserName = request.UserName,
                RoleId = request.RoleId,
                DepartmentId = request.DepartmentId,
                JobPositionId = request.JobPositionId,
                IsActive = true,
            };

            user.PasswordHash = _passwordHasher.HashPassword(user, request.Password);

            //DB Store
            await _userRepository.AddAsync(user);

            var createdUser = await _userRepository.GetByIdAsync(user.Id);

            if (createdUser is null)
            {
                throw new InvalidOperationException("Failed to retrieve created user.");
            }

            //Response DTO
            return new UserDto
            {
                Id = user.Id,
                LoginId = user.LoginId,
                UserName = user.UserName,
                RoleId = user.RoleId,
                RoleName = user.Role?.RoleName ?? string.Empty,
                DepartmentId = user.DepartmentId,
                DepartmentName = user.Department?.DepartmentName ?? string.Empty,
                JobPositionId = user.JobPositionId,
                JobPositionName = user.JobPosition?.JobName ?? string.Empty,
                IsActive = user.IsActive,
                CreatedAt = user.CreatedAt
            };
        }

        public async Task<UserDto?> UpdateAsync(Guid id, UpdateUserRequest request)
        {
            var user = await _userRepository.GetByIdAsync(id);
            if (user == null)
            {
                throw new NotFoundException($"User with ID '{id}' not found.");
            }
            user.UserName = request.UserName;
            user.RoleId = request.RoleId;
            user.DepartmentId = request.DepartmentId;
            user.JobPositionId = request.JobPositionId;
            user.IsActive = request.IsActive;

            if (!string.IsNullOrWhiteSpace(request.Password))
            {
                user.PasswordHash = _passwordHasher.HashPassword(user, request.Password);
            }

            user.UpdatedAt = DateTime.UtcNow;

            await _userRepository.UpdateAsync(user);

            var updatedUser = await _userRepository.GetByIdAsync(id);
            if (updatedUser is null) {
                throw new InvalidOperationException($"Failed to retrieve updated user with ID '{id}'.");
            }

            return new UserDto
            {
                Id = id,
                LoginId = updatedUser.LoginId,
                UserName = updatedUser.UserName,
                RoleId = updatedUser.RoleId,
                RoleName = updatedUser.Role?.RoleName ?? string.Empty,
                DepartmentId = updatedUser.DepartmentId,
                DepartmentName = updatedUser.Department?.DepartmentName ?? string.Empty,
                JobPositionId = updatedUser.JobPositionId,
                JobPositionName = updatedUser.JobPosition?.JobName ?? string.Empty,
                IsActive = updatedUser.IsActive,
                CreatedAt = updatedUser.CreatedAt
            };
        }
    }
}
