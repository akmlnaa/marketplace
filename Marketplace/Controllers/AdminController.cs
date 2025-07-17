using Marketplace.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace Marketplace.Controllers
{
    public class AdminController : Controller
    {
        private readonly AppDbContext _context;

        public AdminController(AppDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var totalUsers = _context.Users.Count();
            var totalIkan = _context.Ikans.Count();
            var totalTransaksi = _context.Transakses.Count();
            var totalToko = _context.Tokos.Count();

            ViewBag.TotalUsers = totalUsers;
            ViewBag.TotalIkan = totalIkan;
            ViewBag.TotalTransaksi = totalTransaksi;
            ViewBag.TotalToko = totalToko;

            return View();
        }

        public IActionResult Users()
        {
            var users = _context.Users.ToList();
            return View(users);
        }

        public IActionResult Ikans()
        {
            var ikans = _context.Ikans
                .Include(i => i.Penjual)
                .Include(i => i.Toko)
                .ToList();

            return View(ikans);
        }


        public IActionResult Transakses()
        {
            var transakses = _context.Transakses
                .Include(t => t.Ikan)
                .Include(t => t.Pembeli)
                .ToList();
            return View(transakses);
        }

        public IActionResult Tokos()
        {
            var tokos = _context.Tokos.Include(t => t.Penjual).ToList();
            return View(tokos);
        }

        public IActionResult Ratings()
        {
            var ratings = _context.Ratings
                .Include(r => r.Ikan)
                .Include(r => r.Pembeli)
                .ToList();
            return View(ratings);
        }
    }
}
