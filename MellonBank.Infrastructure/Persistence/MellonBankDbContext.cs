using MellonBank.Domain.Entities;
using MellonBank.Infrastructure.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace MellonBank.Infrastructure.Persistence
{
    public class MellonBankDbContext : IdentityDbContext<ApplicationUser>
    {
        public MellonBankDbContext(DbContextOptions<MellonBankDbContext> options)
            : base(options)
        {
        }

        public DbSet<BankAccount> BankAccounts { get; set; }
        public DbSet<Transaction> Transactions { get; set; }
        public DbSet<Currency> Currencies { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfigurationsFromAssembly(typeof(MellonBankDbContext).Assembly);
        }
    }
}
