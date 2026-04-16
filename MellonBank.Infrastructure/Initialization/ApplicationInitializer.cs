using MellonBank.Infrastructure.Persistence;
using MellonBank.Infrastructure.Seed;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace MellonBank.Infrastructure.Initialization
{
    public static class ApplicationInitializer
    {
        public static async Task InitializeAsync(IServiceProvider services, IConfiguration config)
        {
            var dbContext = services.GetRequiredService<MellonBankDbContext>();

            await dbContext.Database.MigrateAsync();

            await IdentitySeeder.SeedAsync(services, config);
            await CurrencySeeder.SeedAsync(dbContext);
        }
    }
}
