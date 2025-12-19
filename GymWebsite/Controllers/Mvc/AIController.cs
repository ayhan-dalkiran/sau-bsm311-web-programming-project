using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GymWebsite.Controllers.Mvc;

public class AIController : Controller {
    public ActionResult Index()
    {
        return View();
    }

    public ActionResult Details(int id)
    {
        return View();
    }

    [Authorize(Roles = "Admin")]
    public ActionResult Create()
    {
        return View();
    }

    [Authorize(Roles = "Admin")]
    public ActionResult Edit(int id)
    {
        return View();
    }

    [Authorize(Roles = "Admin")]
    public ActionResult Delete(int id)
    {
        return View();
    }
}
