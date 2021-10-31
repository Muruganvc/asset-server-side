using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

#nullable disable

namespace Asset.models
{
    public partial class AssetContext : DbContext
    {
        public AssetContext()
        {
        }

        public AssetContext(DbContextOptions<AssetContext> options)
            : base(options)
        {
        }

        public virtual DbSet<TblAsset> TblAssets { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlServer("Data Source=CHIDAMBARAMM1;Initial Catalog=Asset;User ID=sa;Password=Admin@123;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "SQL_Latin1_General_CP1_CI_AS");

            modelBuilder.Entity<TblAsset>(entity =>
            {
                entity.HasKey(e => e.AssetId)
                    .HasName("PK__Tbl_Asse__D28B561DB986D40A");

                entity.ToTable("Tbl_Asset");

                entity.Property(e => e.AssetId)
                    .HasColumnName("asset_id")
                    .HasDefaultValueSql("(newid())");

                entity.Property(e => e.Country)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("country");

                entity.Property(e => e.CreatedBy)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("created_by");

                entity.Property(e => e.Description)
                    .HasMaxLength(500)
                    .IsUnicode(false)
                    .HasColumnName("description");

                entity.Property(e => e.Email)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("email");

                entity.Property(e => e.FileName)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("file_name");

                entity.Property(e => e.MimeType)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("mime_type");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
