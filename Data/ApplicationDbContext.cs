using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ControlFinanzasProject.Models;
using Microsoft.EntityFrameworkCore;

namespace ControlFinanzasProject.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
        {
        }

        public DbSet<TipoMovimiento> TipoMovimiento { get; set; }
        public DbSet<CategoriaMovimiento> CategoriaMovimiento { get; set; }
        public DbSet<Movimiento> Movimiento { get; set; }
        public DbSet<TarjetaCredito> TarjetaCredito { get; set; }
        public DbSet<PagoTarjeta> PagoTarjeta { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure relationships

            // CategoriaMovimiento -> TipoMovimiento
            modelBuilder.Entity<CategoriaMovimiento>()
                .HasOne(cm => cm.TipoMovimiento)
                .WithMany(tm => tm.CategoriaMovimientos)
                .HasForeignKey(cm => cm.TipoMovimientoId)
                .OnDelete(DeleteBehavior.Restrict);

            // Movimiento -> CategoriaMovimiento
            modelBuilder.Entity<Movimiento>()
                .HasOne(m => m.CategoriaMovimiento)
                .WithMany(cm => cm.Movimientos)
                .HasForeignKey(m => m.CategoriaMovimientoId)
                .OnDelete(DeleteBehavior.Restrict);

            // Movimiento -> TarjetaCredito (opcional - solo para gastos a crédito)
            modelBuilder.Entity<Movimiento>()
                .HasOne(m => m.TarjetaCredito)
                .WithMany(tc => tc.GastosCredito)
                .HasForeignKey(m => m.TarjetaCreditoId)
                .OnDelete(DeleteBehavior.Restrict);

            // PagoTarjeta -> TarjetaCredito
            modelBuilder.Entity<PagoTarjeta>()
                .HasOne(pt => pt.TarjetaCredito)
                .WithMany(tc => tc.Pagos)
                .HasForeignKey(pt => pt.TarjetaCreditoId)
                .OnDelete(DeleteBehavior.Restrict);

            // PagoTarjeta -> Movimiento (relación uno a uno)
            modelBuilder.Entity<PagoTarjeta>()
                .HasOne(pt => pt.Movimiento)
                .WithOne()
                .HasForeignKey<PagoTarjeta>(pt => pt.MovimientoId)
                .OnDelete(DeleteBehavior.Cascade);

            // Configuraciones adicionales para mejor performance

            // Índices para Movimiento
            modelBuilder.Entity<Movimiento>()
                .HasIndex(m => m.Fecha);

            modelBuilder.Entity<Movimiento>()
                .HasIndex(m => m.EsEfectivo);

            modelBuilder.Entity<Movimiento>()
                .HasIndex(m => m.CategoriaMovimientoId);

            modelBuilder.Entity<Movimiento>()
                .HasIndex(m => m.TarjetaCreditoId);

            modelBuilder.Entity<Movimiento>()
                .HasIndex(m => m.EsActivo);

            // Índices compuestos para consultas comunes
            modelBuilder.Entity<Movimiento>()
                .HasIndex(m => new { m.Fecha, m.EsActivo });

            modelBuilder.Entity<Movimiento>()
                .HasIndex(m => new { m.CategoriaMovimientoId, m.EsActivo });

            // Índices para CategoriaMovimiento
            modelBuilder.Entity<CategoriaMovimiento>()
                .HasIndex(cm => cm.TipoMovimientoId);

            modelBuilder.Entity<CategoriaMovimiento>()
                .HasIndex(cm => cm.EsActivo);

            // Índices para TarjetaCredito
            modelBuilder.Entity<TarjetaCredito>()
                .HasIndex(tc => tc.EsActivo);

            modelBuilder.Entity<TarjetaCredito>()
                .HasIndex(tc => new { tc.Nombre, tc.EsActivo })
                .IsUnique()
                .HasFilter("[EsActivo] = 1");

            // Índices para PagoTarjeta
            modelBuilder.Entity<PagoTarjeta>()
                .HasIndex(pt => pt.TarjetaCreditoId);

            modelBuilder.Entity<PagoTarjeta>()
                .HasIndex(pt => pt.MovimientoId)
                .IsUnique();

            modelBuilder.Entity<PagoTarjeta>()
                .HasIndex(pt => pt.FechaPago);

            // Configuraciones de precisión decimal
            modelBuilder.Entity<Movimiento>()
                .Property(m => m.Monto)
                .HasPrecision(18, 2);

            modelBuilder.Entity<Movimiento>()
                .Property(m => m.SaldoAnterior)
                .HasPrecision(18, 2);

            modelBuilder.Entity<Movimiento>()
                .Property(m => m.SaldoPosterior)
                .HasPrecision(18, 2);

            modelBuilder.Entity<TarjetaCredito>()
                .Property(tc => tc.LimiteCredito)
                .HasPrecision(18, 2);

            modelBuilder.Entity<TarjetaCredito>()
                .Property(tc => tc.DeudaActual)
                .HasPrecision(18, 2);

            modelBuilder.Entity<PagoTarjeta>()
                .Property(pt => pt.MontoPago)
                .HasPrecision(18, 2);

            // Configuraciones de valores por defecto
            modelBuilder.Entity<Movimiento>()
                .Property(m => m.Fecha)
                .HasDefaultValueSql("GETDATE()");

            modelBuilder.Entity<Movimiento>()
                .Property(m => m.EsActivo)
                .HasDefaultValue(true);

            modelBuilder.Entity<Movimiento>()
                .Property(m => m.EsEfectivo)
                .HasDefaultValue(true);

            modelBuilder.Entity<CategoriaMovimiento>()
                .Property(cm => cm.EsActivo)
                .HasDefaultValue(true);

            modelBuilder.Entity<TipoMovimiento>()
                .Property(tm => tm.EsActivo)
                .HasDefaultValue(true);

            modelBuilder.Entity<TarjetaCredito>()
                .Property(tc => tc.EsActivo)
                .HasDefaultValue(true);

            modelBuilder.Entity<PagoTarjeta>()
                .Property(pt => pt.EsActivo)
                .HasDefaultValue(true);

            // Configuración de strings requeridos
            modelBuilder.Entity<TipoMovimiento>()
                .Property(tm => tm.Descripcion)
                .IsRequired()
                .HasMaxLength(100);

            modelBuilder.Entity<CategoriaMovimiento>()
                .Property(cm => cm.Descripcion)
                .IsRequired()
                .HasMaxLength(100);

            modelBuilder.Entity<Movimiento>()
                .Property(m => m.Descripcion)
                .IsRequired()
                .HasMaxLength(200);

            modelBuilder.Entity<TarjetaCredito>()
                .Property(tc => tc.Nombre)
                .IsRequired()
                .HasMaxLength(100);

            // 🔥 NUEVO: Validaciones de negocio
            modelBuilder.Entity<TarjetaCredito>()
                .ToTable(t => t.HasCheckConstraint("CK_TarjetaCredito_DeudaActual", "[DeudaActual] >= 0 AND [DeudaActual] <= [LimiteCredito]"));

            modelBuilder.Entity<Movimiento>()
                .ToTable(t => t.HasCheckConstraint("CK_Movimiento_Monto", "[Monto] > 0"));

            modelBuilder.Entity<PagoTarjeta>()
                .ToTable(t => t.HasCheckConstraint("CK_PagoTarjeta_MontoPago", "[MontoPago] > 0"));
        }

        // Método para seed data inicial (opcional)
        public async Task SeedDataAsync()
        {
            if (!TipoMovimiento.Any())
            {
                var tiposMovimiento = new List<TipoMovimiento>
                {
                    new TipoMovimiento { Descripcion = "Ingreso", EsActivo = true },
                    new TipoMovimiento { Descripcion = "Gasto", EsActivo = true }
                };

                await TipoMovimiento.AddRangeAsync(tiposMovimiento);
                await SaveChangesAsync();
            }
        }
    }
}