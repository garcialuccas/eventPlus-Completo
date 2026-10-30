using System;
using System.Collections.Generic;
using EventPlus.WebAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace EventPlus.WebAPI.BdContextEvent;

public partial class EventContext : DbContext
{
    public EventContext()
    {
    }

    public EventContext(DbContextOptions<EventContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Comentario> Comentario { get; set; }

    public virtual DbSet<Evento> Evento { get; set; }

    public virtual DbSet<Instituicao> Instituicao { get; set; }

    public virtual DbSet<Presenca> Presenca { get; set; }

    public virtual DbSet<StatusEvento> StatusEvento { get; set; }

    public virtual DbSet<TipoEvento> TipoEvento { get; set; }

    public virtual DbSet<TipoUsuario> TipoUsuario { get; set; }

    public virtual DbSet<Usuario> Usuario { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Data Source=D04S22-1252886\\MSSQLSERVER2; initial catalog =EventPlus; User ID=sa;Password=Senai@134;TrustServerCertificate=True;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Comentario>(entity =>
        {
            entity.HasKey(e => e.ComentarioId).HasName("PK__Comentar__F18449582CFBA84B");

            entity.Property(e => e.ComentarioId).HasDefaultValueSql("(newid())");

            entity.HasOne(d => d.Evento).WithMany(p => p.Comentario).HasConstraintName("FK__Comentari__Event__71D1E811");

            entity.HasOne(d => d.Usuario).WithMany(p => p.Comentario).HasConstraintName("FK__Comentari__Usuar__72C60C4A");
        });

        modelBuilder.Entity<Evento>(entity =>
        {
            entity.HasKey(e => e.EventoId).HasName("PK__Evento__1EEB5901E51C063D");

            entity.ToTable(tb => tb.HasTrigger("trgEventoStatusEvento"));

            entity.Property(e => e.EventoId).HasDefaultValueSql("(newid())");

            entity.HasOne(d => d.IdStatusEventoNavigation).WithMany(p => p.Evento).HasConstraintName("FK_Evento_StatusEvento");

            entity.HasOne(d => d.Instituicao).WithMany(p => p.Evento).HasConstraintName("FK__Evento__Institui__6D0D32F4");

            entity.HasOne(d => d.TipoEvento).WithMany(p => p.Evento).HasConstraintName("FK__Evento__TipoEven__6C190EBB");
        });

        modelBuilder.Entity<Instituicao>(entity =>
        {
            entity.HasKey(e => e.Idinstituicao).HasName("PK__Institui__4E7E9AF35056927C");

            entity.Property(e => e.Idinstituicao).HasDefaultValueSql("(newid())");
        });

        modelBuilder.Entity<Presenca>(entity =>
        {
            entity.HasKey(e => e.PresencaId).HasName("PK__Presenca__738EF01A97CDEF4C");

            entity.Property(e => e.PresencaId).HasDefaultValueSql("(newid())");

            entity.HasOne(d => d.Evento).WithMany(p => p.Presenca).HasConstraintName("FK__Presenca__Evento__778AC167");

            entity.HasOne(d => d.Usuario).WithMany(p => p.Presenca).HasConstraintName("FK__Presenca__Usuari__76969D2E");
        });

        modelBuilder.Entity<StatusEvento>(entity =>
        {
            entity.Property(e => e.IdStatusEvento).ValueGeneratedNever();
        });

        modelBuilder.Entity<TipoEvento>(entity =>
        {
            entity.HasKey(e => e.IdtipoEvento).HasName("PK__TipoEven__EF36F33ED0255A71");

            entity.Property(e => e.IdtipoEvento).HasDefaultValueSql("(newid())");
        });

        modelBuilder.Entity<TipoUsuario>(entity =>
        {
            entity.HasKey(e => e.IdtipoUsuario).HasName("PK__TipoUsua__53289754503A9150");

            entity.Property(e => e.IdtipoUsuario).HasDefaultValueSql("(newid())");
        });

        modelBuilder.Entity<Usuario>(entity =>
        {
            entity.HasKey(e => e.UsuarioId).HasName("PK__Usuario__2B3DE798AE505B67");

            entity.Property(e => e.UsuarioId).HasDefaultValueSql("(newid())");

            entity.HasOne(d => d.TipoUsuario).WithMany(p => p.Usuario).HasConstraintName("FK__Usuario__TipoUsu__68487DD7");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
