using System;
using System.Collections.Generic;
using Marketplace.Models;
using Microsoft.EntityFrameworkCore;

namespace Marketplace.Data;

public partial class AppDbContext : DbContext
{
    public AppDbContext()
    {
    }

    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Ikan> Ikans { get; set; }

    public virtual DbSet<Pembayaran> Pembayarans { get; set; }

    public virtual DbSet<Rating> Ratings { get; set; }

    public virtual DbSet<Toko> Tokos { get; set; }

    public virtual DbSet<Transaksi> Transakses { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=LAPTOP-0P0KAFVC\\SQLEXPRESS;Database=MarketplaceDb;Trusted_Connection=True;TrustServerCertificate=True;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Ikan>(entity =>
        {
            entity.Property(e => e.Gambar).HasMaxLength(255);
            entity.Property(e => e.Harga).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.TanggalPublish).HasColumnType("datetime");

            entity.HasOne(d => d.Toko).WithMany(p => p.Ikans)
                .HasForeignKey(d => d.TokoId)
                .HasConstraintName("FK_Ikans_Tokos");
        });

        modelBuilder.Entity<Pembayaran>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Pembayar__3214EC27F7C30285");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.BuktiBayar).HasMaxLength(255);
            entity.Property(e => e.JumlahBayar).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.MetodePembayaran).HasMaxLength(50);
            entity.Property(e => e.Status).HasMaxLength(20);
            entity.Property(e => e.TglPembayaran).HasColumnType("datetime");
            entity.Property(e => e.TransaksiId).HasColumnName("TransaksiID");

            entity.HasOne(d => d.Transaksi).WithMany(p => p.Pembayarans)
                .HasForeignKey(d => d.TransaksiId)
                .HasConstraintName("FK_Pembayaran_Transaksi");
        });

        modelBuilder.Entity<Rating>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Rating__3214EC07477E8844");

            entity.ToTable("Rating");

            entity.Property(e => e.Tanggal)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");

            entity.HasOne(d => d.Ikan).WithMany(p => p.Ratings)
                .HasForeignKey(d => d.IkanId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Rating_Ikan");

            entity.HasOne(d => d.Pembeli).WithMany(p => p.Ratings)
                .HasForeignKey(d => d.PembeliId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Rating_Pembeli");
        });

        modelBuilder.Entity<Toko>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Tokos__3214EC07AF922A9D");

            entity.Property(e => e.Alamat)
                .HasMaxLength(200)
                .IsUnicode(false);
            entity.Property(e => e.NamaToko)
                .HasMaxLength(100)
                .IsUnicode(false);

            entity.HasOne(d => d.Penjual).WithMany(p => p.Tokos)
                .HasForeignKey(d => d.PenjualId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Toko_User");
        });

        modelBuilder.Entity<Transaksi>(entity =>
        {
            entity.ToTable("Transaksis");

            entity.Property(e => e.Status)
                .HasMaxLength(50)
                .HasDefaultValue("Keranjang");
            entity.Property(e => e.TotalHarga).HasColumnType("decimal(18, 2)");

            entity.HasOne(d => d.Ikan).WithMany(p => p.Transaksis)
                .HasForeignKey(d => d.IkanId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Transaksis_Ikan");

            entity.HasOne(d => d.Pembeli).WithMany(p => p.Transaksis)
                .HasForeignKey(d => d.PembeliId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Transaksis_User");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.Property(e => e.NamaLengkap).HasMaxLength(100);
            entity.Property(e => e.NoHp).HasMaxLength(20);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
