using System;
using System.Collections.Generic;

namespace Marketplace.Models;

public partial class Toko
{
    public int Id { get; set; }

    public string NamaToko { get; set; } = null!;

    public string Alamat { get; set; } = null!;

    public int PenjualId { get; set; }

    public string Deskripsi { get; set; } = null!;

    public virtual ICollection<Ikan> Ikans { get; set; } = new List<Ikan>();

    public virtual User? Penjual { get; set; } = null!;
}
