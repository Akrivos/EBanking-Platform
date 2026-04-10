using System;
using System.Collections.Generic;
using System.Text;

namespace MellonBank.Application.Interfaces
{
    public interface IProfileService
    {
        Task ChangePasswordAsync(
        string userId,
        ChangePasswordRequest request,
        CancellationToken cancellationToken = default);
    }
}
