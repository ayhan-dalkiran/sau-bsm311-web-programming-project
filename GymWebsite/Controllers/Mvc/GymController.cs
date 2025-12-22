using GymWebsite.Data;
using GymWebsite.Models.Business;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GymWebsite.Controllers.Mvc;

public class GymController(ApplicationDbContext context) : Controller {
    public ActionResult Index() => RedirectToAction("Details", new { id = context.Gyms.Single().Id });

    public ActionResult Details(int id) {
        var gym = context.Gyms.Include(g => g.Services).SingleOrDefault();

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
