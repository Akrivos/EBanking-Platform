using MellonBank.Application.Interfaces.Services;
using System.Security.Claims;

namespace MellonBank.Web.Services
{
    public class CurrentUserService : ICurrentUserService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CurrentUserService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public string? UserId =>
            _httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier);

        public bool IsInRole(string role) =>
            _httpContextAccessor.HttpContext?.User?.IsInRole(role) ?? false;
    }
}
