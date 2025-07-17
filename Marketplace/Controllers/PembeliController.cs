using Marketplace.Data;
using Marketplace.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;

namespace Marketplace.Controllers
{
    public class PembeliController : Controller
    {
        private readonly AppDbContext _context;

        public PembeliController(AppDbContext context)
        {
            _context = context;
        }

        public IActionResult Index(string search, string sort)
        {
            var query = _context.Ikans
                .Include(i => i.Ratings) 
                .AsQueryable();

            if (!string.IsNullOrEmpty(search))
            {
                query = query.Where(i => i.NamaIkan.Contains(search));
            }

            switch (sort)
            {
                case "harga_asc":
                    query = query.OrderBy(i => i.Harga);
                    break;
                case "harga_desc":
                    query = query.OrderByDescending(i => i.Harga);
                    break;
            }

            ViewBag.CurrentSearch = search;
            ViewBag.CurrentSort = sort;

            var daftarIkan = query.ToList();

            return View(daftarIkan);
        }

        public IActionResult Profile()
        {
            
            var userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null)
            {
                return RedirectToAction("Login", "Account");
            }

            var user = _context.Users.FirstOrDefault(u => u.Id == userId);
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        
        public IActionResult Riwayat()
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null)
            {
                return RedirectToAction("Login", "Account");
            }

            var riwayat = _context.Transakses
    .Include(t => t.Ikan)
    .Include(t => t.Pembayarans) 
    .Where(t => t.PembeliId == userId)
    .OrderByDescending(t => t.Tanggal)
    .ToList();


            return View(riwayat);
        }
        [HttpPost]
        public IActionResult KonfirmasiTerima(int id)
        {
            var transaksi = _context.Transakses
                .FirstOrDefault(t => t.Id == id && t.Status == "Dikirim" && t.PembeliId == HttpContext.Session.GetInt32("UserId"));
            if (transaksi == null) return NotFound();

            transaksi.Status = "Selesai";
            _context.SaveChanges();

            return RedirectToAction("Riwayat");
        }


    }
}
