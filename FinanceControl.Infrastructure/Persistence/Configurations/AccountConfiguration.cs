using FinanceControl.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FinanceControl.Infrastructure.Persistence.Configurations;

public class AccountConfiguration : IEntityTypeConfiguration<Account>
{
    public void Configure(EntityTypeBuilder<Account> builder)
    {
        builder.ConfigureBase();
        
        builder.ToTable("Accounts");
        
        builder.Property(a => a.Name)
            .IsRequired()
            .HasMaxLength(100);
        
        builder.Property(a => a.Type)
            .IsRequired()
            .HasConversion<string>()
            .HasMaxLength(50);

        builder.Property(a => a.Currency)
            .IsRequired()
            .HasMaxLength(3);
        
        builder.OwnsOne(a => a.Balance, money =>
        {
            money.Property(m => m.Amount)
                .HasColumnName("Balance")
                .HasPrecision(18, 2)
                .IsRequired();

            money.Property(m => m.Currency)
                .HasColumnName("BalanceCurrency")
                .HasMaxLength(3)
                .IsRequired();
            
            builder.Property(a => a.IsActive)
                .IsRequired()
                .HasDefaultValue(true);
            
            builder.HasOne(a => a.User)
                .WithMany(u => u.Accounts)
                .HasForeignKey(a => a.UserId)
                .OnDelete(DeleteBehavior.Restrict);
            
            builder.HasMany(a => a.Transactions)
                .WithOne(t => t.Account)
                .HasForeignKey(t => t.AccountId)
                .OnDelete(DeleteBehavior.Cascade);
            
            builder.Ignore(a => a.DomainEvents);

            // Índices
            builder.HasIndex(a => a.Name);
            builder.HasIndex(a => a.UserId);
            builder.HasIndex(a => a.IsActive);
        });
    }
}