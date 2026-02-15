using Microsoft.EntityFrameworkCore;
using SistemaControleEstacionamento.Domain.Entities;

namespace SistemaControleEstacionamento.Infrastructure.Data;

public class EstacionamentoDbContext : DbContext
{
    public EstacionamentoDbContext(DbContextOptions<EstacionamentoDbContext> options)
        : base(options)
    {
    }

    public DbSet<Veiculo> Veiculos { get; set; } = null!;
    public DbSet<Sessao> Sessoes { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Configuração Veiculo
        modelBuilder.Entity<Veiculo>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Placa)
                .IsRequired()
                .HasMaxLength(10);
            entity.HasIndex(e => e.Placa)
                .IsUnique();
            entity.Property(e => e.Modelo)
                .HasMaxLength(100);
            entity.Property(e => e.Cor)
                .HasMaxLength(50);
            
            // Configurar DateTimeOffset para armazenar como UTC
            entity.Property(e => e.CreatedAt)
                .HasConversion(
                    v => v.ToUniversalTime().DateTime,
                    v => new DateTimeOffset(v, TimeSpan.Zero));
            entity.Property(e => e.UpdatedAt)
                .HasConversion(
                    v => v.HasValue ? v.Value.ToUniversalTime().DateTime : (DateTime?)null,
                    v => v.HasValue ? new DateTimeOffset(v.Value, TimeSpan.Zero) : (DateTimeOffset?)null);
        });

        // Configuração Sessao
        modelBuilder.Entity<Sessao>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.HasOne(e => e.Veiculo)
                .WithMany(v => v.Sessoes)
                .HasForeignKey(e => e.VeiculoId)
                .OnDelete(DeleteBehavior.Restrict);
            
            // RowVersion para concorrência otimista (SQLite não suporta IsRowVersion)
            entity.Property(e => e.RowVersion)
                .IsRequired()
                .HasMaxLength(36);
            
            // Unique Index filtrado para garantir apenas uma sessão ativa por veículo
            entity.HasIndex(e => e.VeiculoId)
                .IsUnique()
                .HasDatabaseName("IX_Sessoes_VeiculoId_Ativa_Unique")
                .HasFilter("[Ativa] = 1");
            
            entity.Property(e => e.ValorCobrado)
                .HasPrecision(18, 2);
            
            // Configurar DateTimeOffset para armazenar como UTC
            entity.Property(e => e.DataHoraEntrada)
                .HasConversion(
                    v => v.ToUniversalTime().DateTime,
                    v => new DateTimeOffset(v, TimeSpan.Zero));
            entity.Property(e => e.DataHoraSaida)
                .HasConversion(
                    v => v.HasValue ? v.Value.ToUniversalTime().DateTime : (DateTime?)null,
                    v => v.HasValue ? new DateTimeOffset(v.Value, TimeSpan.Zero) : (DateTimeOffset?)null);
            entity.Property(e => e.CreatedAt)
                .HasConversion(
                    v => v.ToUniversalTime().DateTime,
                    v => new DateTimeOffset(v, TimeSpan.Zero));
            entity.Property(e => e.UpdatedAt)
                .HasConversion(
                    v => v.HasValue ? v.Value.ToUniversalTime().DateTime : (DateTime?)null,
                    v => v.HasValue ? new DateTimeOffset(v.Value, TimeSpan.Zero) : (DateTimeOffset?)null);
        });
    }
}

