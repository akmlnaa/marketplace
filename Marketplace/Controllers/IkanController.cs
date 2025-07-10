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
        public async Task<IActionResult> Create(Ikan ikan, IFormFile GambarFile)
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

                if (GambarFile != null && GambarFile.Length > 0)
                {
                    var fileName = Guid.NewGuid() + Path.GetExtension(GambarFile.FileName);
                    var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images", fileName);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await GambarFile.CopyToAsync(stream);
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
        public IActionResult Edit(Ikan ikan)
        {
            if (ikan.PenjualId != GetLoggedInUser()?.Id)
                return Unauthorized();

            if (ModelState.IsValid)
            {
                _context.Ikans.Update(ikan);
                _context.SaveChanges();
                return RedirectToAction(nameof(Index));
            }

            return View(ikan);
        }

        public IActionResult Delete(int id)
        {
            var ikan = _context.Ikans.Find(id);
            if (ikan == null || ikan.PenjualId != GetLoggedInUser()?.Id)
                return NotFound();

            return View(ikan);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            var ikan = _context.Ikans.Find(id);
            if (ikan != null && ikan.PenjualId == GetLoggedInUser()?.Id)
            {
                _context.Ikans.Remove(ikan);
                _context.SaveChanges();
            }
            return RedirectToAction(nameof(Index));
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
