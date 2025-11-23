using FinanceControl.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FinanceControl.Infrastructure.Persistence.Configurations;

public static class EntityBaseConfiguration
{
    public static void ConfigureBase<TEntity>(this EntityTypeBuilder<TEntity> builder)
        where TEntity : EntityBase
    {
        builder.HasKey(e => e.Id);

        // Auditoria de Criação
        builder.Property(e => e.CreatedAt)
            .IsRequired();

        builder.Property(e => e.CreatedByUserId)
            .IsRequired();
        
        
        // Auditoria de Atualização
        builder.Property(e => e.UpdatedAt);

        builder.Property(e => e.UpdatedByUserId);
        
        // IsActive
        builder.Property(e => e.IsActive)
            .IsRequired()
            .HasDefaultValue(true);
        
        // Soft Delete
        builder.Property(e => e.IsDeleted)
            .IsRequired()
            .HasDefaultValue(false);

        builder.Property(e => e.DeletedAt);

        builder.Property(e => e.DeletedByUserId);
        
        // Query Filter Global - NUNCA retorna registros deletados por padrão
        builder.HasQueryFilter(e => !e.IsDeleted);
        
        // Índices para auditoria
        builder.HasIndex(e => e.CreatedAt);
        builder.HasIndex(e => e.CreatedByUserId);
        builder.HasIndex(e => e.IsActive);
        builder.HasIndex(e => e.IsDeleted);
    }
}