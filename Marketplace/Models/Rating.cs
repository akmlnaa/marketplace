using System;
using System.Collections.Generic;

namespace Marketplace.Models;

public partial class Rating
{
    public int Id { get; set; }

    public int PembeliId { get; set; }

    public int IkanId { get; set; }

    public int Nilai { get; set; }

    public string? Komentar { get; set; }

    public DateTime Tanggal { get; set; }

    public virtual Ikan? Ikan { get; set; } = null!;

    public virtual User? Pembeli { get; set; } = null!;
}
