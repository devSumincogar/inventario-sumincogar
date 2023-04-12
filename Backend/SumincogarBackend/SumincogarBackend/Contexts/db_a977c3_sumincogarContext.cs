using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using SumincogarBackend.Models;

namespace SumincogarBackend.Contexts
{
    public partial class db_a977c3_sumincogarContext : IdentityDbContext
    {
        public db_a977c3_sumincogarContext()
        {
        }

        public db_a977c3_sumincogarContext(DbContextOptions<db_a977c3_sumincogarContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Catalogo> Catalogos { get; set; } = null!;
        public virtual DbSet<Categorium> Categoria { get; set; } = null!;
        public virtual DbSet<Detalleinventario> Detalleinventarios { get; set; } = null!;
        public virtual DbSet<Imagenreferencial> Imagenreferencials { get; set; } = null!;
        public virtual DbSet<Parametrotecnico> Parametrotecnicos { get; set; } = null!;
        public virtual DbSet<Producto> Productos { get; set; } = null!;
        public virtual DbSet<Promocion> Promocions { get; set; } = null!;
        public virtual DbSet<Promocionimagen> Promocionimagens { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {            
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Catalogo>(entity =>
            {
                entity.ToTable("CATALOGO");

                entity.Property(e => e.Catalogoid).HasColumnName("CATALOGOID");

                entity.Property(e => e.Catalogourl)
                    .IsUnicode(false)
                    .HasColumnName("CATALOGOURL");

                entity.Property(e => e.Imagenreferencial)
                    .IsUnicode(false)
                    .HasColumnName("IMAGENREFERENCIAL");

                entity.Property(e => e.Nombre)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("NOMBRE");
            });

            modelBuilder.Entity<Categorium>(entity =>
            {
                entity.HasKey(e => e.Categoriaid);

                entity.ToTable("CATEGORIA");

                entity.Property(e => e.Categoriaid).HasColumnName("CATEGORIAID");

                entity.Property(e => e.Categorianombre)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("CATEGORIANOMBRE");
            });

            modelBuilder.Entity<Detalleinventario>(entity =>
            {
                entity.ToTable("DETALLEINVENTARIO");

                entity.Property(e => e.Detalleinventarioid).HasColumnName("DETALLEINVENTARIOID");

                entity.Property(e => e.Codcliente)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("CODCLIENTE");

                entity.Property(e => e.Codproducto)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("CODPRODUCTO");

                entity.Property(e => e.Colores)
                    .IsUnicode(false)
                    .HasColumnName("COLORES");

                entity.Property(e => e.Impresion)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("IMPRESION");

                entity.Property(e => e.Stock)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("STOCK");
            });

            modelBuilder.Entity<Imagenreferencial>(entity =>
            {
                entity.ToTable("IMAGENREFERENCIAL");

                entity.HasIndex(e => e.Productoid, "RELATIONSHIP_2_FK");

                entity.Property(e => e.Imagenreferencialid).HasColumnName("IMAGENREFERENCIALID");

                entity.Property(e => e.Imagenreferencialurl)
                    .IsUnicode(false)
                    .HasColumnName("IMAGENREFERENCIALURL");

                entity.Property(e => e.Productoid).HasColumnName("PRODUCTOID");

                entity.HasOne(d => d.Producto)
                    .WithMany(p => p.Imagenreferencials)
                    .HasForeignKey(d => d.Productoid)
                    .HasConstraintName("FK_IMAGENRE_RELATIONS_PRODUCTO");
            });

            modelBuilder.Entity<Parametrotecnico>(entity =>
            {
                entity.ToTable("PARAMETROTECNICO");

                entity.HasIndex(e => e.Productoid, "RELATIONSHIP_3_FK");

                entity.Property(e => e.Parametrotecnicoid).HasColumnName("PARAMETROTECNICOID");

                entity.Property(e => e.Clave)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("CLAVE");

                entity.Property(e => e.Productoid).HasColumnName("PRODUCTOID");

                entity.Property(e => e.Valor)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("VALOR");

                entity.HasOne(d => d.Producto)
                    .WithMany(p => p.Parametrotecnicos)
                    .HasForeignKey(d => d.Productoid)
                    .HasConstraintName("FK_PARAMETR_RELATIONS_PRODUCTO");
            });

            modelBuilder.Entity<Producto>(entity =>
            {
                entity.ToTable("PRODUCTO");

                entity.HasIndex(e => e.Categoriaid, "RELATIONSHIP_1_FK");

                entity.Property(e => e.Productoid).HasColumnName("PRODUCTOID");

                entity.Property(e => e.Categoriaid).HasColumnName("CATEGORIAID");

                entity.Property(e => e.Codigo)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("CODIGO");

                entity.Property(e => e.Fichatenicapdf)
                    .IsUnicode(false)
                    .HasColumnName("FICHATENICAPDF");

                entity.Property(e => e.Imagenreferencial)
                    .IsUnicode(false)
                    .HasColumnName("IMAGENREFERENCIAL");

                entity.Property(e => e.Productonombre)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("PRODUCTONOMBRE");

                entity.HasOne(d => d.Categoria)
                    .WithMany(p => p.Productos)
                    .HasForeignKey(d => d.Categoriaid)
                    .HasConstraintName("FK_PRODUCTO_RELATIONS_CATEGORI");
            });

            modelBuilder.Entity<Promocion>(entity =>
            {
                entity.ToTable("PROMOCION");

                entity.Property(e => e.Promocionid).HasColumnName("PROMOCIONID");

                entity.Property(e => e.Fechacaducidad)
                    .HasColumnType("datetime")
                    .HasColumnName("FECHACADUCIDAD");

                entity.Property(e => e.Fechaingreso)
                    .HasColumnType("datetime")
                    .HasColumnName("FECHAINGRESO");

                entity.Property(e => e.Imagenprincipal)
                    .IsUnicode(false)
                    .HasColumnName("IMAGENPRINCIPAL");

                entity.Property(e => e.Prioridad).HasColumnName("PRIORIDAD");

                entity.Property(e => e.Titulo)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("TITULO");
            });

            modelBuilder.Entity<Promocionimagen>(entity =>
            {
                entity.ToTable("PROMOCIONIMAGEN");

                entity.HasIndex(e => e.Promocionid, "RELATIONSHIP_4_FK");

                entity.Property(e => e.Promocionimagenid).HasColumnName("PROMOCIONIMAGENID");

                entity.Property(e => e.Orden).HasColumnName("ORDEN");

                entity.Property(e => e.Promocionid).HasColumnName("PROMOCIONID");

                entity.Property(e => e.Promocionimagenurl)
                    .IsUnicode(false)
                    .HasColumnName("PROMOCIONIMAGENURL");

                entity.HasOne(d => d.Promocion)
                    .WithMany(p => p.Promocionimagens)
                    .HasForeignKey(d => d.Promocionid)
                    .HasConstraintName("FK_PROMOCIO_RELATIONS_PROMOCIO");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
