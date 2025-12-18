using Microsoft.AspNetCore.Mvc;

namespace GymWebsite.Controllers
{
    public class AppointmentsController : Controller
    {
        // Randevu Al Sayfasını Gösterir (GET)
        public IActionResult Create()
        {
            return View();
        }
    }
}