using Marketplace.Data;
using Marketplace.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace Marketplace.Controllers
{
    public class TokoController : Controller
    {
        private readonly AppDbContext _context;

        public TokoController(AppDbContext context)
        {
            _context = context;
        }
        
        private int GetPenjualId()
        {
            var username = HttpContext.Session.GetString("Username");
            var user = _context.Users.FirstOrDefault(u => u.Username == username && u.Role == "Penjual");
            return user?.Id ?? 0;
        }

        // GET: /Toko
        public IActionResult Index()
        {
            int penjualId = GetPenjualId();

            if (penjualId == 0)
            {
                return Unauthorized(); // atau redirect ke login
            }

            var tokoList = _context.Tokos
                .Include(t => t.Penjual)
                .Where(t => t.PenjualId == penjualId)
                .ToList();

            var ikanList = _context.Ikans
                .Where(i => i.PenjualId == penjualId)
                .ToList();

            ViewBag.IkanList = ikanList;

            return View(tokoList); // <- ini Model yang dikirim ke View
        }


        public IActionResult Create()
        {
            int penjualId = GetPenjualId();
            var existingToko = _context.Tokos.FirstOrDefault(t => t.PenjualId == penjualId);

            if (existingToko != null)
            {
                // Jika toko sudah ada, langsung redirect ke index toko
                return RedirectToAction("Index", "Toko");
            }

            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create([Bind("NamaToko,Alamat,Deskripsi")] Toko toko)
        {
            int penjualId = GetPenjualId();
            if (penjualId == 0)
                return Unauthorized();

            if (ModelState.IsValid)
            {
                toko.PenjualId = penjualId;
                _context.Tokos.Add(toko);
                _context.SaveChanges();
                return RedirectToAction("Index", "Toko");
            }

            return View(toko);
        }






        // GET: /Toko/Edit/5
        public IActionResult Edit(int id)
        {
            var toko = _context.Tokos.Find(id);
            if (toko == null || toko.PenjualId != GetPenjualId())
                return NotFound();

            return View(toko);
        }

        // POST: /Toko/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Toko toko)
        {
            if (toko.PenjualId != GetPenjualId())
                return Unauthorized();

            if (ModelState.IsValid)
            {
                _context.Tokos.Update(toko);
                _context.SaveChanges();
                return RedirectToAction(nameof(Index));
            }

            return View(toko);
        }

        // GET: /Toko/Delete/5
        public IActionResult Delete(int id)
        {
            var toko = _context.Tokos.Find(id);
            if (toko == null || toko.PenjualId != GetPenjualId())
                return NotFound();

            return View(toko);
        }

        // POST: /Toko/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            var toko = _context.Tokos.Find(id);
            if (toko != null && toko.PenjualId == GetPenjualId())
            {
                _context.Tokos.Remove(toko);
                _context.SaveChanges();
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
