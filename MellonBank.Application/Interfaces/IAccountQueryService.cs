namespace MellonBank.Application.Interfaces
{
    public interface IAccountQueryService
    {
        Task<IReadOnlyList<MyAccountListItemDto>> GetUserAccountsAsync(string userId, CancellationToken cancellationToken = default);
        Task<AccountBalanceDto?> GetAccountBalanceAsync(string userId, string accountNumber, CancellationToken cancellationToken = default);
        Task<AccountDetailsDto?> GetMyAccountDetailsAsync(string userId, string accountNumber, CancellationToken cancellationToken = default);
    }
}
