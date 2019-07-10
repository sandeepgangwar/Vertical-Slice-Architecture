using System;
using VerticalSliceArchitecture.Core.Domain.Identity;

namespace VerticalSliceArchitecture.Core.Services
{
    public interface IJwtHandler : IService
    {
        string CreateToken(Guid userId, string login, UserRole role);
    }
}