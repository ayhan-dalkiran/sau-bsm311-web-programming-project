using Microsoft.AspNetCore.Mvc;

namespace GymWebsite.Controllers;
public class TrainersController : Controller {
    public IActionResult Index() {
        return View();
    }
}
