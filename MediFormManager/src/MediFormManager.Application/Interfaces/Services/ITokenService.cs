using MediFormManager.Domain.Entities.Users;
using System;
using System.Collections.Generic;
using System.Text;

namespace MediFormManager.Application.Interfaces.Services
{
    public interface ITokenService
    {
        string GenerateToken(User user);
    }
}
