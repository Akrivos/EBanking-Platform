using EBanking.Application.Interfaces.Persistence;
using EBanking.Application.Interfaces.Repositories;
using EBanking.Application.Interfaces.Services;
using EBanking.Infrastructure.Identity;
using EBanking.Infrastructure.Options;
using EBanking.Infrastructure.Persistence;
using EBanking.Infrastructure.Persistence.Repositories;
using EBanking.Infrastructure.Persistence.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;


namespace EBanking.Infrastructure
{
    public static class DependencyInjection
    {
        public static void AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("DefaultConnection") ?? 
                throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

            services.AddDbContext<EBankingDbContext>(options => options.UseSqlServer(connectionString));

            services.AddIdentity<ApplicationUser, IdentityRole>(options =>
            {
                options.SignIn.RequireConfirmedAccount = false;
                options.Password.RequiredLength = 6;
                options.Password.RequireNonAlphanumeric = true;
                options.Password.RequireDigit = true;
                options.Password.RequireUppercase = true;
                options.Password.RequireLowercase = true;
            })
            .AddEntityFrameworkStores<EBankingDbContext>()
            .AddDefaultTokenProviders();

            services.Configure<ExchangeRateApiOptions>(configuration.GetSection("ExchangeRateProvider"));

            services.AddHttpClient<IExchangeRateProviderService, ExchangeRateProviderService>();

            services.AddScoped<IIdentityService, IdentityService>();
            services.AddScoped<IRoleService, RoleService>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IBankAccountRepository, BankAccountRepository>();
            services.AddScoped<ITransactionRepository, TransactionRepository>();
            services.AddScoped<ICurrencyRepository, CurrencyRepository>();
        }
    }
}
