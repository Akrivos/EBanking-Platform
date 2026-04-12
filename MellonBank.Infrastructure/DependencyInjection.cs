using MellonBank.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Identity;
using MellonBank.Application.Interfaces.Repositories;
using MellonBank.Application.Interfaces.Persistence;

namespace MellonBank.Infrastructure
{
    public static class DependencyInjection
    {
        public static void AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'AppDbContextConnection' not found.");

            services.AddDbContext<MellonBankDbContext>(options =>
                options.UseSqlServer(connectionString));

            services.AddIdentityCore<IdentityUser>(options =>
                {
                    options.SignIn.RequireConfirmedAccount = false;
                    options.User.RequireUniqueEmail = true;
                    options.Password.RequiredLength = 6;
                    options.Password.RequireNonAlphanumeric = true;
                    options.Password.RequireDigit = true;
                    options.Password.RequireUppercase = true;
                    options.Password.RequireLowercase = true;
                })
                .AddRoles<IdentityRole>()
                .AddEntityFrameworkStores<MellonBankDbContext>();

            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IBankAccountRepository, IBankAccountRepository>();
            services.AddScoped<ITransactionRepository, ITransactionRepository>();
        }
    }
}
