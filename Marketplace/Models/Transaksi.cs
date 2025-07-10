using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Marketplace.Models;

public partial class Transaksi
{
    public int Id { get; set; }

    public int PembeliId { get; set; }

    public int IkanId { get; set; }

    public int Jumlah { get; set; }

    public DateTime Tanggal { get; set; }

    public string? Status { get; set; } = null!;

    public decimal TotalHarga { get; set; }

    public virtual Ikan? Ikan { get; set; } = null!;

    public virtual ICollection<Pembayaran> Pembayarans { get; set; } = new List<Pembayaran>();

    public virtual User? Pembeli { get; set; } = null!;
}
