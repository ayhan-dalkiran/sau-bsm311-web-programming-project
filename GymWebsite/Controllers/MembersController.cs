using Microsoft.AspNetCore.Mvc;

namespace GymWebsite.Controllers;
public class MembersController : Controller {
    public IActionResult Index() {
        return View();
    }
}
