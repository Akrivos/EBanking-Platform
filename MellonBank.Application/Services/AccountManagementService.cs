using MellonBank.Application.Interfaces.Services;

namespace MellonBank.Application.Services
{
    public class AccountManagementService : IAccountManagementService
    {
        private readonly IAccountManagementService _accountManagementService;

        public AccountManagementService(IAccountManagementService accountManagementService)
        {
            _accountManagementService = accountManagementService;
        }

        //public Task CreateBankAccountAsync(string userId, string branch, string accountType, string currency)
        //{
        //    return _accountManagementService.CreateBankAccountAsync(userId, branch, accountType, currency);
        //}
    }
}
