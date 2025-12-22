using GymWebsite.Data;
using GymWebsite.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace GymWebsite.Controllers.Mvc;

public class HomeController : Controller {
    private readonly ILogger<HomeController> _logger;
    private readonly ApplicationDbContext _context; // Veritabaný baðlantýsý eklendi

    public HomeController(ILogger<HomeController> logger, ApplicationDbContext context) {
        _logger = logger;
        _context = context;
    }

    public IActionResult Index() {
        // Veritabanýndaki ÝLK salonu ve onun hizmetlerini çekiyoruz.
        var gymInfo = _context.Gyms
                              .Include(g => g.Services) // Hizmetleri de getir (Join)
                              .FirstOrDefault();

        // Eðer veritabaný boþsa hata vermesin diye boþ bir nesne oluþtur.
        if (gymInfo == null) gymInfo = new Models.Business.Gym { Name = "GYM Website" };

        return View(gymInfo);
    }

    public IActionResult Privacy() {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error() {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}