using Marketplace.Data;
using Marketplace.Models;
using Microsoft.AspNetCore.Mvc;

namespace Marketplace.Controllers
{
    public class RatingController : Controller
    {
        private readonly AppDbContext _context;

        public RatingController(AppDbContext context)
        {
            _context = context;
        }

        private int GetPembeliId()
        {
            var username = HttpContext.Session.GetString("Username");
            var user = _context.Users.FirstOrDefault(u => u.Username == username && u.Role == "Pembeli");
            return user?.Id ?? 0;
        }

        // GET: Rating/Beri/5 (5 = IkanId)
        [HttpGet]
        public IActionResult Beri(int ikanId)
        {
            var pembeliId = GetPembeliId();
            if (pembeliId == 0) return RedirectToAction("Login", "Account");

            var sudahRating = _context.Ratings.Any(r => r.IkanId == ikanId && r.PembeliId == pembeliId);
            if (sudahRating)
            {
                TempData["Error"] = "Kamu sudah memberi rating untuk ikan ini.";
                return RedirectToAction("Riwayat", "Transaksi");
            }

            var rating = new Rating
            {
                IkanId = ikanId
            };

            return View(rating);
        }

        // POST: Rating/Beri
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Beri(Rating rating)
        {
            var pembeliId = GetPembeliId();
            if (pembeliId == 0) return RedirectToAction("Login", "Account");

            rating.PembeliId = pembeliId;
            rating.Tanggal = DateTime.Now;

            if (ModelState.IsValid)
            {
                _context.Ratings.Add(rating);
                _context.SaveChanges();
                TempData["Success"] = "Rating berhasil diberikan.";
                return RedirectToAction("Riwayat", "Transaksi");
            }

            return View(rating);
        }
    }
}
