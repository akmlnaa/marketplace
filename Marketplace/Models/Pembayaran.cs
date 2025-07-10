using System;
using System.Collections.Generic;

namespace Marketplace.Models;

public partial class Pembayaran
{
    public int Id { get; set; }

    public int TransaksiId { get; set; }

    public DateTime TglPembayaran { get; set; }

    public string MetodePembayaran { get; set; } = null!;

    public decimal JumlahBayar { get; set; }

    public string? BuktiBayar { get; set; }

    public string Status { get; set; } = null!;

    public virtual Transaksi? Transaksi { get; set; } = null!;
}
