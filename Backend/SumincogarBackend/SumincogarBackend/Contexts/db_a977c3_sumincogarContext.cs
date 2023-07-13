using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using SumincogarBackend.Models;

namespace SumincogarBackend.Contexts
{
    public partial class db_a977c3_sumincogarContext : DbContext
    {
        public db_a977c3_sumincogarContext()
        {
        }

        public db_a977c3_sumincogarContext(DbContextOptions<db_a977c3_sumincogarContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Catalogo> Catalogo { get; set; } = null!;
        public virtual DbSet<Categoria> Categoria { get; set; } = null!;
        public virtual DbSet<Detalleinventario> Detalleinventario { get; set; } = null!;
        public virtual DbSet<Fichatecnica> Fichatecnica { get; set; } = null!;
        public virtual DbSet<GamaColor> GamaColor { get; set; } = null!;
        public virtual DbSet<Imagenreferencial> Imagenreferencial { get; set; } = null!;
        public virtual DbSet<Parametrotecnico> Parametrotecnico { get; set; } = null!;
        public virtual DbSet<Producto> Producto { get; set; } = null!;
        public virtual DbSet<ProductoGamacolor> ProductoGamacolor { get; set; } = null!;
        public virtual DbSet<Promocion> Promocion { get; set; } = null!;
        public virtual DbSet<Promocionimagen> Promocionimagen { get; set; } = null!;
        public virtual DbSet<SubCategoria> SubCategoria { get; set; } = null!;
        public virtual DbSet<Usuario> Usuario { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Catalogo>(entity =>
            {
                entity.ToTable("CATALOGO");

                entity.Property(e => e.CatalogoId).HasColumnName("CATALOGO_ID");

                entity.Property(e => e.ImagenUrl)
                    .IsUnicode(false)
                    .HasColumnName("IMAGEN_URL");

                entity.Property(e => e.Nombre)
                    .IsUnicode(false)
                    .HasColumnName("NOMBRE");

                entity.Property(e => e.Url)
                    .IsUnicode(false)
                    .HasColumnName("URL");
            });

            modelBuilder.Entity<Categoria>(entity =>
            {
                entity.ToTable("CATEGORIA");

                entity.Property(e => e.CategoriaId).HasColumnName("CATEGORIA_ID");

                entity.Property(e => e.CategoriaNombre)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("CATEGORIA_NOMBRE");
            });

            modelBuilder.Entity<Detalleinventario>(entity =>
            {
                entity.ToTable("DETALLEINVENTARIO");

                entity.Property(e => e.DetalleInventarioId).HasColumnName("DETALLE_INVENTARIO_ID");

                entity.Property(e => e.CodCliente)
                    .IsUnicode(false)
                    .HasColumnName("COD_CLIENTE");

                entity.Property(e => e.CodProducto)
                    .IsUnicode(false)
                    .HasColumnName("COD_PRODUCTO");

                entity.Property(e => e.Colores)
                    .IsUnicode(false)
                    .HasColumnName("COLORES");

                entity.Property(e => e.Descontinuada)
                    .IsRequired()
                    .HasColumnName("DESCONTINUADA")
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.Impresion)
                    .IsUnicode(false)
                    .HasColumnName("IMPRESION");

                entity.Property(e => e.Stock)
                    .IsUnicode(false)
                    .HasColumnName("STOCK");

                entity.Property(e => e.TelasSimilares)
                    .IsUnicode(false)
                    .HasColumnName("TELAS_SIMILARES")
                    .HasDefaultValueSql("('')");
            });

            modelBuilder.Entity<Fichatecnica>(entity =>
            {
                entity.ToTable("FICHATECNICA");

                entity.HasIndex(e => e.CategoriaId, "RELATIONSHIP_3_FK");

                entity.Property(e => e.FichaTecnicaId).HasColumnName("FICHA_TECNICA_ID");

                entity.Property(e => e.CategoriaId).HasColumnName("CATEGORIA_ID");

                entity.Property(e => e.DocumentoUrl)
                    .IsUnicode(false)
                    .HasColumnName("DOCUMENTO_URL");

                entity.Property(e => e.NombreFichaTecnica)
                    .IsUnicode(false)
                    .HasColumnName("NOMBRE_FICHA_TECNICA");

                entity.HasOne(d => d.Categoria)
                    .WithMany(p => p.Fichatecnica)
                    .HasForeignKey(d => d.CategoriaId)
                    .HasConstraintName("FK_FICHATEC_RELATIONS_CATEGORI");
            });

            modelBuilder.Entity<GamaColor>(entity =>
            {
                entity.ToTable("GAMA_COLOR");

                entity.Property(e => e.GamaColorId).HasColumnName("GAMA_COLOR_ID");

                entity.Property(e => e.GamaColorNombre)
                    .IsUnicode(false)
                    .HasColumnName("GAMA_COLOR_NOMBRE");
            });

            modelBuilder.Entity<Imagenreferencial>(entity =>
            {
                entity.HasKey(e => e.ImagenReferenciaId);

                entity.ToTable("IMAGENREFERENCIAL");

                entity.HasIndex(e => e.ProductoId, "RELATIONSHIP_2_FK");

                entity.Property(e => e.ImagenReferenciaId).HasColumnName("IMAGEN_REFERENCIA_ID");

                entity.Property(e => e.ProductoId).HasColumnName("PRODUCTO_ID");

                entity.Property(e => e.Url)
                    .IsUnicode(false)
                    .HasColumnName("URL");

                entity.HasOne(d => d.Producto)
                    .WithMany(p => p.Imagenreferencial)
                    .HasForeignKey(d => d.ProductoId)
                    .HasConstraintName("FK_IMAGENRE_RELATIONS_PRODUCTO");
            });

            modelBuilder.Entity<Parametrotecnico>(entity =>
            {
                entity.ToTable("PARAMETROTECNICO");

                entity.HasIndex(e => e.FichaTecnicaId, "RELATIONSHIP_5_FK");

                entity.Property(e => e.ParametroTecnicoId).HasColumnName("PARAMETRO_TECNICO_ID");

                entity.Property(e => e.Clave)
                    .IsUnicode(false)
                    .HasColumnName("CLAVE");

                entity.Property(e => e.FichaTecnicaId).HasColumnName("FICHA_TECNICA_ID");

                entity.Property(e => e.Valor)
                    .IsUnicode(false)
                    .HasColumnName("VALOR");

                entity.HasOne(d => d.FichaTecnica)
                    .WithMany(p => p.Parametrotecnico)
                    .HasForeignKey(d => d.FichaTecnicaId)
                    .HasConstraintName("FK_PARAMETR_RELATIONS_FICHATEC");
            });

            modelBuilder.Entity<Producto>(entity =>
            {
                entity.ToTable("PRODUCTO");

                entity.Property(e => e.ProductoId).HasColumnName("PRODUCTO_ID");

                entity.Property(e => e.Codigo)
                    .IsUnicode(false)
                    .HasColumnName("CODIGO");

                entity.Property(e => e.ImagenUrl)
                    .IsUnicode(false)
                    .HasColumnName("IMAGEN_URL");

                entity.Property(e => e.ProductoNombre)
                    .IsUnicode(false)
                    .HasColumnName("PRODUCTO_NOMBRE");

                entity.Property(e => e.SubcategoriaId).HasColumnName("SUBCATEGORIA_ID");

                entity.HasOne(d => d.Subcategoria)
                    .WithMany(p => p.Producto)
                    .HasForeignKey(d => d.SubcategoriaId)
                    .HasConstraintName("FK_SUBCATEGORIA_PRODUCTO");
            });

            modelBuilder.Entity<ProductoGamacolor>(entity =>
            {
                entity.HasKey(e => new { e.ProductoId, e.GamaColorId })
                    .HasName("PK__PRODUCTO__27242E4706208E83");

                entity.ToTable("PRODUCTO_GAMACOLOR");

                entity.Property(e => e.ProductoId).HasColumnName("PRODUCTO_ID");

                entity.Property(e => e.GamaColorId).HasColumnName("GAMA_COLOR_ID");
            });

            modelBuilder.Entity<Promocion>(entity =>
            {
                entity.ToTable("PROMOCION");

                entity.Property(e => e.PromocionId).HasColumnName("PROMOCION_ID");

                entity.Property(e => e.FechaCaducidad)
                    .HasColumnType("datetime")
                    .HasColumnName("FECHA_CADUCIDAD");

                entity.Property(e => e.FechaIngreso)
                    .HasColumnType("datetime")
                    .HasColumnName("FECHA_INGRESO");

                entity.Property(e => e.ImagenPrincipal)
                    .IsUnicode(false)
                    .HasColumnName("IMAGEN_PRINCIPAL");

                entity.Property(e => e.Prioridad).HasColumnName("PRIORIDAD");

                entity.Property(e => e.Titulo)
                    .IsUnicode(false)
                    .HasColumnName("TITULO");
            });

            modelBuilder.Entity<Promocionimagen>(entity =>
            {
                entity.ToTable("PROMOCIONIMAGEN");

                entity.HasIndex(e => e.PromocionId, "RELATIONSHIP_4_FK");

                entity.Property(e => e.PromocionImagenId).HasColumnName("PROMOCION_IMAGEN_ID");

                entity.Property(e => e.Orden).HasColumnName("ORDEN");

                entity.Property(e => e.PromocionId).HasColumnName("PROMOCION_ID");

                entity.Property(e => e.Url)
                    .IsUnicode(false)
                    .HasColumnName("URL");

                entity.HasOne(d => d.Promocion)
                    .WithMany(p => p.Promocionimagen)
                    .HasForeignKey(d => d.PromocionId)
                    .HasConstraintName("FK_PROMOCIO_RELATIONS_PROMOCIO");
            });

            modelBuilder.Entity<SubCategoria>(entity =>
            {
                entity.ToTable("SUB_CATEGORIA");

                entity.Property(e => e.SubcategoriaId).HasColumnName("SUBCATEGORIA_ID");

                entity.Property(e => e.CategoriaId).HasColumnName("CATEGORIA_ID");

                entity.Property(e => e.SubcategoriaNombre)
                    .IsUnicode(false)
                    .HasColumnName("SUBCATEGORIA_NOMBRE");

                entity.HasOne(d => d.Categoria)
                    .WithMany(p => p.SubCategoria)
                    .HasForeignKey(d => d.CategoriaId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_CATEGORIA_ID");
            });

            modelBuilder.Entity<Usuario>(entity =>
            {
                entity.ToTable("USUARIO");

                entity.Property(e => e.UsuarioId).HasColumnName("USUARIO_ID");

                entity.Property(e => e.Apellido)
                    .HasMaxLength(500)
                    .IsUnicode(false)
                    .HasColumnName("APELLIDO");

                entity.Property(e => e.Nombre)
                    .HasMaxLength(500)
                    .IsUnicode(false)
                    .HasColumnName("NOMBRE");

                entity.Property(e => e.Tutorial).HasColumnName("TUTORIAL");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
