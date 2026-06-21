using EBanking.Domain.Entities;
using EBanking.Infrastructure.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EBanking.Infrastructure.Persistence.Configurations
{
    public class TransactionConfiguration : IEntityTypeConfiguration<Transaction>
    {
        public void Configure(EntityTypeBuilder<Transaction> builder)
        {
            builder.ToTable("Transactions");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Amount)
                .IsRequired()
                .HasPrecision(18, 2);

            builder.Property(x => x.Description)
                .HasMaxLength(250);

            builder.Property(x => x.Type)
                .IsRequired()
                .HasConversion<int>();

            builder.Property(x => x.Status)
                .IsRequired()
                .HasConversion<int>();

            builder.Property(x => x.ExecutedByUserId)
                .IsRequired();

            builder.Property(x => x.ReferenceCode)
                .IsRequired()
                .HasMaxLength(20);

            builder.HasIndex(x => x.ReferenceCode)
                .IsUnique();

            builder.HasOne(x => x.FromAccount)
                .WithMany()
                .HasForeignKey(x => x.FromAccountId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.ToAccount)
                .WithMany()
                .HasForeignKey(x => x.ToAccountId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne<ApplicationUser>()
                .WithMany()
                .HasForeignKey(x => x.ExecutedByUserId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}