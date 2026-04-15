using FluentValidation;
using MellonBank.Application.Interfaces.Services;
using MellonBank.Application.Services;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace MellonBank.Application
{
    public static class DependencyInjection
    {
        public static void AddApplication(this IServiceCollection services)
        {
            services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

            services.AddScoped<IStaffUserManagementService, StaffUserManagementService>();
            services.AddScoped<ICustomerAccountService, CustomerAccountService>();
            services.AddScoped<ICurrencyService, CurrencyService>();
            services.AddScoped<IProfileService, ProfileService>();
            services.AddScoped<IAccountManagementService, AccountManagementService>();
            services.AddScoped<ITransferService, TransferService>();
        }
    }
}
