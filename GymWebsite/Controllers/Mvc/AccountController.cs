using Microsoft.AspNetCore.Mvc;

namespace GymWebsite.Controllers.Mvc; 
public class AccountController : Controller {
    public IActionResult Register() {
        return View();
    }
}