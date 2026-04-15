using MellonBank.Domain.Entities;
using MellonBank.Infrastructure.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MellonBank.Infrastructure.Persistence.Configurations
{
    public class BankAccountConfiguration : IEntityTypeConfiguration<BankAccount>
    {
        public void Configure(EntityTypeBuilder<BankAccount> builder)
        {
            builder.ToTable("BankAccounts");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.AccountNumber)
                .IsRequired()
                .HasMaxLength(20);

            builder.HasIndex(x => x.AccountNumber)
                .IsUnique();

            builder.Property(x => x.Balance)
                .HasPrecision(18, 2);

            builder.Property(x => x.Currency)
                .IsRequired()
                .HasConversion<int>();

            builder.Property(x => x.Branch)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(x => x.AccountType)
                .IsRequired()
                .HasConversion<int>();

            builder.Property(x => x.UserId)
                .IsRequired();

            builder.HasOne<ApplicationUser>()
                .WithMany()
                .HasForeignKey(x => x.UserId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}