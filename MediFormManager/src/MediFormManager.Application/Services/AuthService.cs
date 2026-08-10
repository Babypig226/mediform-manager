using MediFormManager.Application.DTOs.Auth;
using MediFormManager.Application.Interfaces.Repositories;
using MediFormManager.Application.Interfaces.Services;
using MediFormManager.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace MediFormManager.Application.Services
{
    public class AuthService
    {
        private readonly IUserRepository _userRepository;
        private readonly ITokenService _tokenService;
        private readonly IPasswordHasher<User> _passwordHasher;

        public AuthService(IUserRepository userRepository, ITokenService tokenService)
        {
            _userRepository = userRepository;
            _tokenService = tokenService;
            _passwordHasher = new PasswordHasher<User>();
        }

        public async Task<LoginResponse?> LoginAsync(LoginRequest request)
        {
            var user = await _userRepository.GetByLoginIdAsync(request.LoginId);
            
            if (user is null)
            {
                return null;
            }

            if (!user.IsActive) {
                return null;
            }

            var passwordVerificationResult = _passwordHasher.VerifyHashedPassword(user, user.PasswordHash, request.Password);
            if (passwordVerificationResult != PasswordVerificationResult.Success)
            {
                return null;
            }
            var token = _tokenService.GenerateToken(user);
            return new LoginResponse
            {
                AccessTocken = token,
                UserId = user.Id,
                LoginId = user.LoginId,
                UserName = user.UserName,
                RoleName = user.Role?.RoleName ?? string.Empty
            };
        }
    }
}
