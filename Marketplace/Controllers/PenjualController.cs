using Marketplace.Data;
using Marketplace.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace Marketplace.Controllers
{
    public class PenjualController : Controller
    {
        private readonly AppDbContext _context;

        public PenjualController(AppDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var username = HttpContext.Session.GetString("Username");
            if (string.IsNullOrEmpty(username)) return RedirectToAction("Login", "Account");

            var penjual = _context.Users.FirstOrDefault(u => u.Username == username);
            if (penjual == null) return Unauthorized();

            var toko = _context.Tokos.FirstOrDefault(t => t.PenjualId == penjual.Id);
            ViewBag.SudahPunyaToko = toko != null;

            return View(penjual);
        }
        public IActionResult Profile()
        {
            var userId = HttpContext.Session.GetInt32("UserId");

            if (userId == null)
            {
                return RedirectToAction("Login", "Account");
            }

            var user = _context.Users.FirstOrDefault(u => u.Id == userId && u.Role == "Penjual");

            if (user == null)
            {
                return NotFound();
            }

            return View(user); 
        }
        public IActionResult PembayaranMasuk()
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null) return RedirectToAction("Login", "Account");

            
            var toko = _context.Tokos.FirstOrDefault(t => t.PenjualId == userId.Value);
            if (toko == null) return NotFound("Toko tidak ditemukan.");

            
            var pembayaranList = _context.Pembayarans
                .Include(p => p.Transaksi).ThenInclude(t => t.Ikan)
                .Include(p => p.Transaksi).ThenInclude(t => t.Pembeli)
                .Where(p =>
                    p.Transaksi.Ikan.TokoId == toko.Id &&
                    (p.Status == "Menunggu Konfirmasi" || p.Status == "Dikonfirmasi" || p.Status == "Ditolak")
                )
                .ToList();

            return View(pembayaranList);
        }


        
        [HttpPost]
        public IActionResult KonfirmasiPembayaran(int id)
        {
            var pembayaran = _context.Pembayarans.Include(p => p.Transaksi).FirstOrDefault(p => p.Id == id);
            if (pembayaran == null) return NotFound();

            pembayaran.Status = "Dikonfirmasi";
            pembayaran.Transaksi.Status = "Dikonfirmasi";
            _context.SaveChanges();

            return RedirectToAction("PembayaranMasuk");
        }

        
        [HttpPost]
        public IActionResult TolakPembayaran(int id)
        {
            var pembayaran = _context.Pembayarans.Include(p => p.Transaksi).FirstOrDefault(p => p.Id == id);
            if (pembayaran == null) return NotFound();

            pembayaran.Status = "Ditolak";
            pembayaran.Transaksi.Status = "Ditolak";
            _context.SaveChanges();

            return RedirectToAction("PembayaranMasuk");
        }
        [HttpPost]
        public IActionResult KirimPesanan(int id)
        {
            var transaksi = _context.Transakses
                .Include(t => t.Ikan)
                .FirstOrDefault(t => t.Id == id && t.Status == "Dikonfirmasi");
            if (transaksi == null) return NotFound();

            transaksi.Status = "Dikirim";
            _context.SaveChanges();

            return RedirectToAction("PembayaranMasuk");
        }
        public IActionResult RiwayatPenjualan()
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null) return RedirectToAction("Login", "Account");

            var toko = _context.Tokos.FirstOrDefault(t => t.PenjualId == userId.Value);
            if (toko == null) return NotFound("Toko tidak ditemukan.");

            var riwayat = _context.Transakses
                .Include(t => t.Ikan)
                .Include(t => t.Pembeli)
                .Include(t => t.Pembayarans)
                .Where(t => t.Ikan.TokoId == toko.Id &&
                            (t.Status == "Dikonfirmasi" || t.Status == "Dikirim"))
                .OrderByDescending(t => t.Tanggal)
                .ToList();

            return View(riwayat); 
        }




    }
}
