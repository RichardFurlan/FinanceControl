using FinanceControl.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FinanceControl.Infrastructure.Persistence.Configurations;

public class CategoryConfiguration : IEntityTypeConfiguration<Category>
{
    public void Configure(EntityTypeBuilder<Category> builder)
    {
        builder.ConfigureBase();
        
        builder.ToTable("Categories");
        
        builder.Property(c => c.Name)
            .IsRequired()
            .HasMaxLength(50);
        
        builder.Property(c => c.Description)
            .IsRequired()
            .HasMaxLength(500);

        builder.Property(c => c.Icon)
            .HasMaxLength(10);

        builder.Property(c => c.Color)
            .HasMaxLength(7);

        builder.Property(c => c.IsActive)
            .IsRequired()
            .HasDefaultValue(true);

        builder.HasOne(c => c.User)
            .WithOne(u => u.Categories)
            .OnDelete(DeleteBehavior.Restrict);

        // Índices
        builder.HasIndex(c => c.Name);
        builder.HasIndex(c => c.IsActive);
    }
}