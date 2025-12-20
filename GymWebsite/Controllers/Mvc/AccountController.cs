using Microsoft.AspNetCore.Mvc;

namespace GymWebsite.Controllers.Mvc {
    public class AccountController : Controller {
        // Sadece Giriş Sayfasını Gösterir (GET)
        public IActionResult Login() {
            return View();
        }
    }
}