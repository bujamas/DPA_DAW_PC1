using System;
using System.Collections.Generic;
using DPA.SERVICIOTECNICO.CORE.Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace DPA.SERVICIOTECNICO.CORE.Infrastructure.Data;

public partial class ServicioTecnicoDbContext : DbContext
{
    public ServicioTecnicoDbContext(DbContextOptions<ServicioTecnicoDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Cliente> Cliente { get; set; }

    public virtual DbSet<Equipo> Equipo { get; set; }

    public virtual DbSet<OrdenReparacion> OrdenReparacion { get; set; }

    public virtual DbSet<TipoReparacion> TipoReparacion { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Cliente>(entity =>
        {
            entity.Property(e => e.Correo).HasMaxLength(150);
            entity.Property(e => e.Materno).HasMaxLength(50);
            entity.Property(e => e.Nombres).HasMaxLength(100);
            entity.Property(e => e.Paterno).HasMaxLength(50);
            entity.Property(e => e.Telefono).HasMaxLength(20);
        });

        modelBuilder.Entity<Equipo>(entity =>
        {
            entity.HasIndex(e => e.ClienteId, "IX_Equipo_ClienteId");

            entity.HasIndex(e => e.NumeroSerie, "UQ_Equipo_NumeroSerie").IsUnique();

            entity.Property(e => e.Marca).HasMaxLength(50);
            entity.Property(e => e.Modelo).HasMaxLength(50);
            entity.Property(e => e.NumeroSerie).HasMaxLength(50);

            entity.HasOne(d => d.Cliente).WithMany(p => p.Equipo)
                .HasForeignKey(d => d.ClienteId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Equipo_Cliente");
        });

        modelBuilder.Entity<OrdenReparacion>(entity =>
        {
            entity.HasIndex(e => e.EquipoId, "IX_OrdenReparacion_EquipoId");

            entity.HasIndex(e => e.TipoReparacionId, "IX_OrdenReparacion_TipoReparacionId");

            entity.Property(e => e.CostoEstimado).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.DescripcionProblema).HasMaxLength(500);
            entity.Property(e => e.Estado)
                .HasMaxLength(20)
                .HasDefaultValue("Pendiente", "DF_OrdenReparacion_Estado");
            entity.Property(e => e.FechaIngreso)
                .HasPrecision(0)
                .HasDefaultValueSql("(sysdatetime())", "DF_OrdenReparacion_FechaIngreso");

            entity.HasOne(d => d.Equipo).WithMany(p => p.OrdenReparacion)
                .HasForeignKey(d => d.EquipoId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_OrdenReparacion_Equipo");

            entity.HasOne(d => d.TipoReparacion).WithMany(p => p.OrdenReparacion)
                .HasForeignKey(d => d.TipoReparacionId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_OrdenReparacion_TipoReparacion");
        });

        modelBuilder.Entity<TipoReparacion>(entity =>
        {
            entity.Property(e => e.Nombre).HasMaxLength(100);
            entity.Property(e => e.PrecioBase).HasColumnType("decimal(10, 2)");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
