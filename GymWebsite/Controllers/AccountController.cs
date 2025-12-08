using Microsoft.AspNetCore.Mvc;

namespace GymWebsite.Controllers {
    public class AccountController : Controller {
        public IActionResult Register() {
            return View();
        }
    }
}