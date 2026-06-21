using EBanking.Application.DTOs.Responses;

namespace EBanking.Application.Interfaces.Services
{
    public interface ICustomerAccountService
    {
        Task<IEnumerable<CustomerAccountDetailsResponseDto>> GetAccountsAsync(CancellationToken ct = default);
        Task<CustomerAccountDetailsResponseDto?> GetAccountDetailsAsync(string accountNumber, CancellationToken ct = default);
        Task<ConversionResponseDto?> GetConvertedBalanceAsync(string accountNumber, CancellationToken ct = default);
    }
}
