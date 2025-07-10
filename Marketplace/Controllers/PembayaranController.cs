using Microsoft.AspNetCore.Mvc;
using Marketplace.Data;
using Marketplace.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
using System.IO;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Marketplace.Controllers
{
    public class PembayaranController : Controller
    {
        private readonly AppDbContext _context;

        public PembayaranController(AppDbContext context)
        {
            _context = context;
        }

        // GET: Pembayaran/FormPembayaran?transaksiId=5
        public IActionResult FormPembayaran(int transaksiId)
        {
            var transaksi = _context.Transakses
                .Include(t => t.Ikan) // kalau kamu pakai navigasi ke Ikan
                .FirstOrDefault(t => t.Id == transaksiId);

            if (transaksi == null)
            {
                return NotFound();
            }

            var pembayaran = new Pembayaran
            {
                TransaksiId = transaksiId,
                TglPembayaran = DateTime.Now,
                Status = "Menunggu Konfirmasi",
                JumlahBayar = transaksi.TotalHarga // otomatis isi dengan harga transaksi
            };

            return View(pembayaran);
        }


        // POST: Pembayaran/FormPembayaran
        [HttpPost]
        public async Task<IActionResult> FormPembayaran(Pembayaran pembayaran, IFormFile? buktiBayarFile)
        {
            pembayaran.TglPembayaran = DateTime.Now;
            pembayaran.Status = "Menunggu Konfirmasi";

            var transaksi = await _context.Transakses.FirstOrDefaultAsync(t => t.Id == pembayaran.TransaksiId);
            if (transaksi == null)
            {
                return NotFound();
            }

            pembayaran.JumlahBayar = transaksi.TotalHarga; // ambil dari DB, bukan form!

            if (pembayaran.MetodePembayaran == "Transfer Bank")
            {
                if (buktiBayarFile == null || buktiBayarFile.Length == 0)
                {
                    ModelState.AddModelError("BuktiBayar", "Bukti bayar wajib diunggah untuk metode Transfer Bank.");
                }
            }

            if (ModelState.IsValid)
            {
                if (pembayaran.MetodePembayaran == "Transfer Bank" && buktiBayarFile != null)
                {
                    var fileName = Guid.NewGuid() + Path.GetExtension(buktiBayarFile.FileName);
                    var uploadPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/uploads/bukti");
                    Directory.CreateDirectory(uploadPath);
                    var filePath = Path.Combine(uploadPath, fileName);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await buktiBayarFile.CopyToAsync(stream);
                    }

                    pembayaran.BuktiBayar = "/uploads/bukti/" + fileName;
                }

                _context.Pembayarans.Add(pembayaran);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Sukses), new { id = pembayaran.Id });
            }

            return View(pembayaran);
        }

        // GET: Pembayaran/Sukses/id
        public async Task<IActionResult> Sukses(int id)
        {
            var pembayaran = await _context.Pembayarans
                .Include(p => p.Transaksi)
                .ThenInclude(t => t.Ikan)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (pembayaran == null)
                return NotFound();

            return View(pembayaran);
        }

        
    }
}
