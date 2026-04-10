namespace MellonBank.Application.Interfaces.Services
{
    public interface ICurrentUserService
    {
        string? UserId { get; }
        bool IsInRole(string role);
    }
}
