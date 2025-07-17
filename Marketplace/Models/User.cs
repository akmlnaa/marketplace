using System;
using System.Collections.Generic;

namespace Marketplace.Models;

public partial class User
{
    public int Id { get; set; }

    public string Username { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string Password { get; set; } = null!;

    public string Role { get; set; } = null!;

    public string? NamaLengkap { get; set; }

    public string? NoHp { get; set; }

    public string? Alamat { get; set; }

    public virtual ICollection<Ikan> Ikans { get; set; } = new List<Ikan>();

    public virtual ICollection<Rating> Ratings { get; set; } = new List<Rating>();

    public virtual ICollection<Toko> Tokos { get; set; } = new List<Toko>();

    public virtual ICollection<Transaksi> Transaksis { get; set; } = new List<Transaksi>();
}
