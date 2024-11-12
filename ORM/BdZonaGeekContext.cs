using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace Zona_Geek.ORM;

public partial class BdZonaGeekContext : DbContext
{
    public BdZonaGeekContext()
    {
    }

    public BdZonaGeekContext(DbContextOptions<BdZonaGeekContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Atendimento> Atendimentos { get; set; }

    public virtual DbSet<Servico> Servicos { get; set; }

    public virtual DbSet<Usuario> Usuarios { get; set; }

    public virtual DbSet<ViewAtendimento> ViewAtendimentos { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=LAB205_14\\SQLEXPRESS;Database=Bd_Zona_Geek;User Id=admin_zeze;Password=Admin00;TrustServerCertificate=true");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Atendimento>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_Agendamento");

            entity.ToTable("Atendimento");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.DtHorarioAgendamento).HasColumnType("datetime");
            entity.Property(e => e.FkServico).HasColumnName("Fk_Servico");
            entity.Property(e => e.FkUsuario).HasColumnName("Fk_Usuario");

            entity.HasOne(d => d.FkServicoNavigation).WithMany(p => p.Atendimentos)
                .HasForeignKey(d => d.FkServico)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Atendimento_Servico");

            entity.HasOne(d => d.FkUsuarioNavigation).WithMany(p => p.Atendimentos)
                .HasForeignKey(d => d.FkUsuario)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Atendimento_Usuario");
        });

        modelBuilder.Entity<Servico>(entity =>
        {
            entity.ToTable("Servico");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.TipoServico)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Valor).HasColumnType("decimal(18, 0)");
        });

        modelBuilder.Entity<Usuario>(entity =>
        {
            entity.ToTable("Usuario");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Email)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Nome)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Senha)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Telefone)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<ViewAtendimento>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("View_Atendimento");

            entity.Property(e => e.DtHorarioAgendamento).HasColumnType("datetime");
            entity.Property(e => e.Email)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Nome)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Telefone)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.TipoServico)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Valor).HasColumnType("decimal(18, 0)");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
