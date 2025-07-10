using Microsoft.AspNetCore.Mvc;
using Marketplace.Data;              
using Marketplace.Models;                 
using System.Linq;                   


public class HomeController : Controller
{
    private readonly AppDbContext _context;

    public HomeController(AppDbContext context)
    {
        _context = context;
    }

    public IActionResult Index()
    {
        var daftarIkan = _context.Ikans
    .OrderByDescending(i => i.TanggalPublish ?? new DateTime(1753, 1, 1))
    .Take(2)
    .ToList();

        var daftarToko = _context.Tokos
            .OrderByDescending(t => t.Id)   
            .Take(1)
            .ToList();

        var viewModel = new HomeViewModel
        {
            DaftarIkan = daftarIkan,
            DaftarToko = daftarToko
        };

        return View(viewModel);
    }
}
