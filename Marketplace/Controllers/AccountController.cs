using Marketplace.Data;
using Marketplace.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace Marketplace.Controllers
{
    public class AccountController : Controller
    {
        private readonly AppDbContext _context;

        public AccountController(AppDbContext context)
        {
            _context = context;
        }

        // GET: /Account/Register
        public IActionResult Register()
        {
            return View();
        }

        // POST: /Account/Register
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                // Cek apakah username sudah ada
                var existingUser = _context.Users
                    .FirstOrDefault(u => u.Username == model.Username);

                if (existingUser != null)
                {
                    ModelState.AddModelError("", "Username sudah digunakan.");
                    return View(model);
                }

                // Pastikan password dan konfirmasi password cocok
                if (model.Password != model.ConfirmPassword)
                {
                    ModelState.AddModelError("ConfirmPassword", "Password dan konfirmasi password tidak cocok.");
                    return View(model);
                }

                // Tambahkan user baru
                var user = new User
                {
                    Username = model.Username,
                    Email = model.Email,
                    Password = model.Password, 
                    Role = model.Role,
                    NamaLengkap = model.NamaLengkap,
                    NoHp = model.NoHp,
                    Alamat = model.Alamat
                };


                _context.Users.Add(user);
                _context.SaveChanges();

                return RedirectToAction("Login", "Account");

            }

            return View(model);
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var existingUser = _context.Users
                    .FirstOrDefault(u => u.Username == model.Username && u.Password == model.Password);

                if (existingUser != null)
                {
                    HttpContext.Session.SetInt32("UserId", existingUser.Id);
                    HttpContext.Session.SetString("Username", existingUser.Username);
                    HttpContext.Session.SetString("Role", existingUser.Role);

                    // Arahkan sesuai role
                    switch (existingUser.Role.ToLower())
                    {
                        case "penjual":
                            return RedirectToAction("Index", "Penjual");
                        case "admin":
                            return RedirectToAction("Index", "Admin");
                        case "pembeli":
                            return RedirectToAction("Index", "Pembeli");
                        default:
                            return RedirectToAction("Index", "Home");
                    }
                }

                ModelState.AddModelError("", "Username atau password salah.");
            }

            return View(model);
        }
        public IActionResult Logout()
            {
                // Hapus semua data session
                HttpContext.Session.Clear();

                // Redirect ke halaman login atau landing page
                return RedirectToAction("Index", "Home");
            }
        }



    }

