using Marketplace.Data;
using Marketplace.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Marketplace.Controllers
{
    public class IkanController : Controller
    {
        private readonly AppDbContext _context;

        public IkanController(AppDbContext context)
        {
            _context = context;
        }

        private User GetLoggedInUser()
        {
            var username = HttpContext.Session.GetString("Username");
            return _context.Users.FirstOrDefault(u => u.Username == username && u.Role == "Penjual");
        }

        private Toko GetToko()
        {
            var user = GetLoggedInUser();
            return user != null ? _context.Tokos.FirstOrDefault(t => t.PenjualId == user.Id) : null;
        }

        public IActionResult Index()
        {
            var user = GetLoggedInUser();
            if (user == null) return RedirectToAction("Login", "Account");

            var ikanList = _context.Ikans.Where(i => i.PenjualId == user.Id).ToList();
            return View(ikanList);
        }

        public IActionResult Create()
        {
            if (GetToko() == null)
                return RedirectToAction("BuatToko", "Penjual");

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Ikan ikan)
        {
            var user = GetLoggedInUser();
            if (user == null) return RedirectToAction("Login", "Account");

            if (ModelState.IsValid)
            {
                ikan.PenjualId = user.Id;

                var toko = _context.Tokos.FirstOrDefault(t => t.PenjualId == user.Id);
                if (toko == null)
                {
                    return RedirectToAction("Create", "Toko");
                }

                ikan.TokoId = toko.Id;

                // GambarFile dari model
                if (ikan.GambarFile != null && ikan.GambarFile.Length > 0)
                {
                    var fileName = Guid.NewGuid() + Path.GetExtension(ikan.GambarFile.FileName);
                    var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images", fileName);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await ikan.GambarFile.CopyToAsync(stream);
                    }

                    ikan.Gambar = "/images/" + fileName;
                }

                _context.Ikans.Add(ikan);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index", "Toko");
            }

            return View(ikan);
        }



        public IActionResult Edit(int id)
        {
            var ikan = _context.Ikans.Find(id);
            if (ikan == null || ikan.PenjualId != GetLoggedInUser()?.Id)
                return NotFound();

            return View(ikan);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Ikan updatedIkan)
        {
            var existingIkan = await _context.Ikans.FindAsync(id);
            var loggedInUser = GetLoggedInUser();

            if (existingIkan == null || existingIkan.PenjualId != loggedInUser?.Id)
                return NotFound();

            if (ModelState.IsValid)
            {
                // Update properti teks
                existingIkan.NamaIkan = updatedIkan.NamaIkan;
                existingIkan.Harga = updatedIkan.Harga;
                existingIkan.Stok = updatedIkan.Stok;
                existingIkan.Deskripsi = updatedIkan.Deskripsi;
                existingIkan.TanggalPublish = DateTime.Now;

                // Jika user upload gambar baru
                if (updatedIkan.GambarFile != null && updatedIkan.GambarFile.Length > 0)
                {
                    var fileName = Guid.NewGuid() + Path.GetExtension(updatedIkan.GambarFile.FileName);
                    var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images", fileName);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await updatedIkan.GambarFile.CopyToAsync(stream);
                    }

                    // Update path gambar
                    existingIkan.Gambar = "/images/" + fileName;
                }

                _context.Ikans.Update(existingIkan);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index", "Toko");
            }

            return View(updatedIkan);
        }


        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            // Temukan ikan sekaligus transaksi terkait
            var ikan = await _context.Ikans
                .Include(i => i.Transaksis)
                .FirstOrDefaultAsync(i => i.Id == id);

            // Cek kepemilikan & keberadaan
            if (ikan == null || ikan.PenjualId != GetLoggedInUser()?.Id)
                return NotFound();

            // Hapus semua transaksi terkait
            _context.Transakses.RemoveRange(ikan.Transaksis);

            // Hapus ikan
            _context.Ikans.Remove(ikan);
            await _context.SaveChangesAsync();

            return RedirectToAction("Index", "Toko");
        }



        public IActionResult Detail(int id)
        {
            var ikan = _context.Ikans
                .Include(i => i.Toko) 
                .FirstOrDefault(i => i.Id == id);

            if (ikan == null)
            {
                return NotFound();
            }

            return View(ikan);
        }



    }
}
