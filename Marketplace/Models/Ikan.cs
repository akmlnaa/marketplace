using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Marketplace.Models;

public partial class Ikan
{
    public int Id { get; set; }

    public string NamaIkan { get; set; } = null!;

    public decimal Harga { get; set; }

    public int Stok { get; set; }

    public int PenjualId { get; set; }

    public string? Gambar { get; set; }

    public string? Deskripsi { get; set; }

    public DateTime? TanggalPublish { get; set; }

    public int? TokoId { get; set; }

    public virtual User Penjual { get; set; } = null!;

    public virtual ICollection<Rating> Ratings { get; set; } = new List<Rating>();

    public virtual Toko? Toko { get; set; }
    [NotMapped]
    public IFormFile? GambarFile { get; set; }

    public virtual ICollection<Transaksi> Transaksis { get; set; } = new List<Transaksi>();
}
