using MediFormManager.Domain.Entities;
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
