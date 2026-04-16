using MellonBank.Domain.Entities;
using MellonBank.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace MellonBank.Infrastructure.Seed
{
    public static class CurrencySeeder
    {
        public static async Task SeedAsync(MellonBankDbContext dbContext)
        {
            if (await dbContext.Currencies.AnyAsync())
            {
                return;
            }

            var currency = new Currency(
                1.6500m,
                0.9200m,
                0.8700m,
                1.1800m
            );

            dbContext.Currencies.Add(currency);
            await dbContext.SaveChangesAsync();
        }
    }
}