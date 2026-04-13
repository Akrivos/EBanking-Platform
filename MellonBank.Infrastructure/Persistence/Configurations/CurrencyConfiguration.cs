using MellonBank.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace MellonBank.Infrastructure.Persistence.Configurations
{
    public class CurrencyConfiguration : IEntityTypeConfiguration<Currency>
    {
        public void Configure(Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder<Currency> builder)
        {
            builder.ToTable("Currencies");

            builder.HasKey(c => c.Id);

            builder.Property(c => c.AUD).IsRequired();

            builder.Property(c => c.CHF).IsRequired();

            builder.Property(c => c.GBP).IsRequired();

            builder.Property(c => c.USD).IsRequired();

            builder.Property(c => c.RetrievedAtUtc).IsRequired();
        }
    }
}
