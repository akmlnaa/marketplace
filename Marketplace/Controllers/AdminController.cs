﻿using Microsoft.AspNetCore.Mvc;
using Marketplace.Data;

public class AdminController : Controller
{
    private readonly AppDbContext _context;

    public AdminController(AppDbContext context)
    {
        _context = context;
    }

    public IActionResult Index()
    {
        return View();
    }

    public IActionResult KelolaPengguna()
    {
        var users = _context.Users.ToList();
        return View(users);
    }
}
