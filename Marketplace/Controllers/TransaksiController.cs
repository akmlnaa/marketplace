using Marketplace.Data;
using Marketplace.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Marketplace.Controllers
{
    public class TransaksiController : Controller
    {
        private readonly AppDbContext _context;

        public TransaksiController(AppDbContext context)
        {
            _context = context;
        }

        // Helper: Ambil ID Pembeli dari Session
        private int GetPembeliId()
        {
            var username = HttpContext.Session.GetString("Username");
            var user = _context.Users.FirstOrDefault(u => u.Username == username && u.Role == "Pembeli");
            return user?.Id ?? 0;
        }

        // GET: Transaksi/AddToCart/5 → tampilkan form tambah ke keranjang
        [HttpGet]
        public IActionResult AddToCart(int ikanId)
        {
            var ikan = _context.Ikans.FirstOrDefault(i => i.Id == ikanId);
            if (ikan == null) return NotFound();

            ViewBag.Ikan = ikan;    

            var transaksi = new Transaksi
            {
                IkanId = ikan.Id,
                Jumlah = 1 // default quantity
            };

            return View(transaksi);
        }


        // POST: Transaksi/AddToCart → simpan ke keranjang
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AddToCart(Transaksi transaksi)
        {
            int pembeliId = GetPembeliId();
            if (pembeliId == 0) return RedirectToAction("Login", "Account");

            var ikan = _context.Ikans.FirstOrDefault(i => i.Id == transaksi.IkanId);
            if (ikan == null) return NotFound();

            transaksi.PembeliId = pembeliId;
            transaksi.Tanggal = DateTime.Now;
            transaksi.Status = "Keranjang";
            transaksi.TotalHarga = ikan.Harga * transaksi.Jumlah;

            if (ModelState.IsValid)
            {
                _context.Transakses.Add(transaksi);
                _context.SaveChanges();
                return RedirectToAction("Cart");
            }

            ViewBag.Ikan = ikan;
            return View(transaksi);
        }

        
        public IActionResult Cart()
        {
            var pembeliId = GetPembeliId();
            if (pembeliId == 0) return RedirectToAction("Login", "Account");

            var keranjang = _context.Transakses
                .Include(t => t.Ikan)
                .Where(t => t.PembeliId == pembeliId && t.Status == "Keranjang")
                .ToList();

            return View(keranjang);
        }

        
        [HttpPost]
        public IActionResult Remove(int id)
        {
            var transaksi = _context.Transakses.FirstOrDefault(t => t.Id == id && t.Status == "Keranjang");
            if (transaksi != null)
            {
                _context.Transakses.Remove(transaksi);
                _context.SaveChanges();
            }

            return RedirectToAction("Cart");
        }

        
        [HttpPost]
        public IActionResult Checkout(List<int> selectedIds)
        {
            var pembeliId = GetPembeliId();
            if (pembeliId == 0) return RedirectToAction("Login", "Account");

            if (selectedIds == null || !selectedIds.Any())
            {
                TempData["Error"] = "Pilih minimal satu item untuk checkout.";
                return RedirectToAction("Cart");
            }

            var keranjang = _context.Transakses
                .Include(t => t.Ikan) 
                .Where(t => selectedIds.Contains(t.Id) && t.PembeliId == pembeliId && t.Status == "Keranjang")
                .ToList();

            foreach (var item in keranjang)
            {
                
                if (item.Ikan != null)
                {
                    if (item.Ikan.Stok < item.Jumlah)
                    {
                        TempData["Error"] = $"Stok ikan '{item.Ikan.NamaIkan}' tidak mencukupi.";
                        return RedirectToAction("Cart");
                    }

                    item.Ikan.Stok -= item.Jumlah;
                    _context.Ikans.Update(item.Ikan);
                }

                item.Status = "Sedang Diproses";
                _context.Transakses.Update(item);
            }

            _context.SaveChanges();

            var transaksiPertama = keranjang.FirstOrDefault();
            if (transaksiPertama == null)
            {
                TempData["Error"] = "Terjadi kesalahan saat proses checkout.";
                return RedirectToAction("Cart");
            }

            return RedirectToAction("FormPembayaran", "Pembayaran", new { transaksiId = transaksiPertama.Id });
        }

    }
}
