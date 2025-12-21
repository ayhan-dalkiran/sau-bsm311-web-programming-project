using GymWebsite.Data;
using GymWebsite.Models.Staff;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GymWebsite.Controllers.Mvc;
public class TrainerController(ApplicationDbContext context) : Controller {
    public ActionResult Index() {
        return View();
    }

    public ActionResult Details(int id) {
        var trainer = context.Trainers.Find(id);

        if (trainer is null) {
            return NotFound();
        }

        return View();
    }

    [Authorize(Roles = "Admin")]
    public ActionResult Create(Trainer trainer) {
        context.Trainers.Add(trainer);
        context.SaveChanges();

        return View();
    }

    [Authorize(Roles = "Admin")]
    public ActionResult Edit(Trainer trainer) {
        context.Trainers.Update(trainer);
        context.SaveChanges();

        return View();
    }

    [Authorize(Roles = "Admin")]
    public ActionResult Delete(int id) {
        var trainer = context.Trainers.Find(id);

        if (trainer is null) {
            return NotFound();
        }

        context.Trainers.Remove(trainer);
        context.SaveChanges();

        return View();
    }
}