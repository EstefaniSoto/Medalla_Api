using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace Medalla_Api.Models;

public partial class MedallaContext : DbContext
{
    public MedallaContext()
    {
    }

    public MedallaContext(DbContextOptions<MedallaContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Candidata> Candidatas { get; set; }

    public virtual DbSet<Categoria> Categorias { get; set; }

    public virtual DbSet<Finalista> Finalistas { get; set; }

    public virtual DbSet<Institucione> Instituciones { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<Usuario> Usuarios { get; set; }

    public virtual DbSet<VotosInstitucione> VotosInstituciones { get; set; }

    public virtual DbSet<VotosNormale> VotosNormales { get; set; }

    public virtual DbSet<VwPodioFinal> VwPodioFinals { get; set; }

    public virtual DbSet<VwPromediosNormale> VwPromediosNormales { get; set; }

    public virtual DbSet<VwPuntosInstitucione> VwPuntosInstituciones { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=10.0.0.11;Database=Medalla_Al_Merito;User Id=estefani;Password=nenita02;TrustServerCertificate=True;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Candidata>(entity =>
        {
            entity.HasKey(e => e.CandidataId).HasName("PK__Candidat__DE53CCDA9A4A677E");

            entity.Property(e => e.FotoUrl)
                .HasMaxLength(500)
                .IsUnicode(false);
            entity.Property(e => e.Nombre)
                .HasMaxLength(150)
                .IsUnicode(false);

            entity.HasOne(d => d.Categoria).WithMany(p => p.Candidata)
                .HasForeignKey(d => d.CategoriaId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Candidata__Categ__4222D4EF");
        });

        modelBuilder.Entity<Categoria>(entity =>
        {
            entity.HasKey(e => e.CategoriaId).HasName("PK__Categori__F353C1E5CB3E9EEF");

            entity.Property(e => e.Descripcion)
                .HasMaxLength(300)
                .IsUnicode(false);
            entity.Property(e => e.Nombre)
                .HasMaxLength(150)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Finalista>(entity =>
        {
            entity.HasKey(e => e.FinalistaId).HasName("PK__Finalist__3064EE10A237B2B4");

            entity.HasIndex(e => e.CandidataId, "UQ__Finalist__DE53CCDB7CF0574E").IsUnique();

            entity.HasOne(d => d.Candidata).WithOne(p => p.Finalista)
                .HasForeignKey<Finalista>(d => d.CandidataId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Finalista__Candi__4CA06362");
        });

        modelBuilder.Entity<Institucione>(entity =>
        {
            entity.HasKey(e => e.InstitucionId).HasName("PK__Instituc__706D41C926430E5A");

            entity.Property(e => e.Nombre)
                .HasMaxLength(200)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.HasKey(e => e.RoleId).HasName("PK__Roles__8AFACE1AF9A8E8EA");

            entity.Property(e => e.Name)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Usuario>(entity =>
        {
            entity.HasKey(e => e.UsuarioId).HasName("PK__Usuarios__2B3DE7B87203B41B");

            entity.HasIndex(e => e.Username, "UQ__Usuarios__536C85E4FCF0A538").IsUnique();

            entity.Property(e => e.Nombre)
                .HasMaxLength(150)
                .IsUnicode(false);
            entity.Property(e => e.PasswordHash)
                .HasMaxLength(300)
                .IsUnicode(false);
            entity.Property(e => e.Username)
                .HasMaxLength(100)
                .IsUnicode(false);

            entity.HasOne(d => d.Institucion).WithMany(p => p.Usuarios)
                .HasForeignKey(d => d.InstitucionId)
                .HasConstraintName("FK__Usuarios__Instit__3D5E1FD2");

            entity.HasOne(d => d.Role).WithMany(p => p.Usuarios)
                .HasForeignKey(d => d.RoleId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Usuarios__RoleId__3C69FB99");
        });

        modelBuilder.Entity<VotosInstitucione>(entity =>
        {
            entity.HasKey(e => e.VotoInstId).HasName("PK__VotosIns__3DAC7208A56AD15A");

            entity.HasIndex(e => new { e.InstitucionId, e.CandidataId }, "UQ_VotoInstitucion").IsUnique();

            entity.Property(e => e.Fecha)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");

            entity.HasOne(d => d.Candidata).WithMany(p => p.VotosInstituciones)
                .HasForeignKey(d => d.CandidataId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__VotosInst__Candi__52593CB8");

            entity.HasOne(d => d.Institucion).WithMany(p => p.VotosInstituciones)
                .HasForeignKey(d => d.InstitucionId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__VotosInst__Insti__5165187F");
        });

        modelBuilder.Entity<VotosNormale>(entity =>
        {
            entity.HasKey(e => e.VotoId).HasName("PK__VotosNor__5D77CA9224F1EDEF");

            entity.HasIndex(e => new { e.UsuarioId, e.CandidataId }, "UQ_VotoNormal").IsUnique();

            entity.Property(e => e.Fecha)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");

            entity.HasOne(d => d.Candidata).WithMany(p => p.VotosNormales)
                .HasForeignKey(d => d.CandidataId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__VotosNorm__Candi__48CFD27E");

            entity.HasOne(d => d.Usuario).WithMany(p => p.VotosNormales)
                .HasForeignKey(d => d.UsuarioId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__VotosNorm__Usuar__47DBAE45");
        });

        modelBuilder.Entity<VwPodioFinal>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("vw_PodioFinal");

            entity.Property(e => e.FotoUrl)
                .HasMaxLength(500)
                .IsUnicode(false);
            entity.Property(e => e.NombreCandidata)
                .HasMaxLength(150)
                .IsUnicode(false);
        });

        modelBuilder.Entity<VwPromediosNormale>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("vw_PromediosNormales");

            entity.Property(e => e.FotoUrl)
                .HasMaxLength(500)
                .IsUnicode(false);
            entity.Property(e => e.NombreCandidata)
                .HasMaxLength(150)
                .IsUnicode(false);
            entity.Property(e => e.Promedio).HasColumnType("decimal(38, 6)");
        });

        modelBuilder.Entity<VwPuntosInstitucione>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("vw_PuntosInstituciones");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
