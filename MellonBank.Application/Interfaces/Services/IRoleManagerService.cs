using MellonBank.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace MellonBank.Application.Interfaces.Services
{
    public interface IRoleManagerService
    {
        Task<bool> RoleExistsAsync(RoleType roleName, CancellationToken ct = default);
    }
}
