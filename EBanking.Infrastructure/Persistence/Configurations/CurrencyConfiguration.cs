using EBanking.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EBanking.Infrastructure.Persistence.Configurations
{
    public class CurrencyConfiguration : IEntityTypeConfiguration<Currency>
    {
        public void Configure(EntityTypeBuilder<Currency> builder)
        {
            builder.ToTable("Currencies");

            builder.HasKey(c => c.Id);

            builder.Property(c => c.AUD).IsRequired().HasPrecision(18, 6);

            builder.Property(c => c.CHF).IsRequired().HasPrecision(18, 6);

            builder.Property(c => c.GBP).IsRequired().HasPrecision(18, 6);

            builder.Property(c => c.USD)
                .IsRequired()
                .HasPrecision(18, 6);

            builder.Property(c => c.RetrievedAtUtc).IsRequired();
        }
    }
}
