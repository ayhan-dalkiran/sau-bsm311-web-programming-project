using Microsoft.AspNetCore.Mvc;

namespace GymWebsite.Controllers.Mvc;
public class MembersController : Controller {
    public IActionResult Index() {
        return View();
    }
}
