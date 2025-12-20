using GymWebsite.Data;
using GymWebsite.Models.Business;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GymWebsite.Controllers.Mvc;

public class GymController(ApplicationDbContext context) : Controller {
    private readonly Gym coreModel = context.Gyms.Find(1)!;

    public ActionResult Index() => RedirectToAction("Details", new { id = coreModel.Id });

    public ActionResult Details(int id) {
        var gym = context.Gyms.Find(id);

        if (gym == null) return NotFound();

        return View(gym);
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
