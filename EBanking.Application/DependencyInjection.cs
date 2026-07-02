using FluentValidation;
using EBanking.Application.Common.Generators;
using EBanking.Application.Interfaces.Services;
using EBanking.Application.Services;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace EBanking.Application
{
    public static class DependencyInjection
    {
        public static void AddApplication(this IServiceCollection services)
        {
            services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

            services.AddTransient<IAccountNumberGenerator, AccountNumberGenerator>();

            services.AddScoped<IStaffUserManagementService, StaffUserManagementService>();
            services.AddScoped<ICustomerAccountService, CustomerAccountService>();
            services.AddScoped<ICurrencyService, CurrencyService>();
            services.AddScoped<IProfileService, ProfileService>();
            services.AddScoped<IAccountManagementService, AccountManagementService>();
            services.AddScoped<ITransferService, TransferService>();
            services.AddScoped<ITransactionService, TransactionService>();
        }
    }
}
