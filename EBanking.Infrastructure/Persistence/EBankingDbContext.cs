using EBanking.Domain.Entities;
using EBanking.Infrastructure.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace EBanking.Infrastructure.Persistence
{
    public class EBankingDbContext : IdentityDbContext<ApplicationUser>
    {
        public EBankingDbContext(DbContextOptions<EBankingDbContext> options)
            : base(options)
        {
        }

        public DbSet<BankAccount> BankAccounts { get; set; }
        public DbSet<Transaction> Transactions { get; set; }
        public DbSet<Currency> Currencies { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfigurationsFromAssembly(typeof(EBankingDbContext).Assembly);
        }
    }
}
