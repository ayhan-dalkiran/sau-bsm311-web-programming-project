using Microsoft.AspNetCore.Mvc;

namespace GymWebsite.Controllers.Mvc;
public class TrainersController : Controller {
    public IActionResult Index() {
        return View();
    }
}
