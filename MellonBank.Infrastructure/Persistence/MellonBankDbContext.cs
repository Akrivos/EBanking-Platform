using Microsoft.EntityFrameworkCore;

namespace MellonBank.Infrastructure.Persistence
{
    public class MellonBankDbContext : DbContext
    {
        public MellonBankDbContext(DbContextOptions<MellonBankDbContext> options)
            : base(options)
        {
        }
        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfigurationsFromAssembly(typeof(MellonBankDbContext).Assembly);
        }
    }
}
