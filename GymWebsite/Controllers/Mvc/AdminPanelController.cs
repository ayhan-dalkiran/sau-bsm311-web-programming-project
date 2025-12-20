using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GymWebsite.Controllers.Mvc;

//[Authorize(Roles = "Admin")] DENEME AMAÇLI YORUM SATIRI YAPTIM BURAYI SON AŞAMADA TEKRAR KOD YAP
public class AdminPanelController : Controller {
    // GET: AdminPanelController
    public ActionResult Index() => View();

    public ActionResult Gyms() => View();

    public ActionResult Users() => View();

    public ActionResult Trainers() => View();

    public ActionResult Appointments() => View();

    // GET: AdminPanelController/Details/5
    public ActionResult Details(int id)
    {
        return View();
    }

    // GET: AdminPanelController/Create
    public ActionResult Create()
    {
        return View();
    }

    // POST: AdminPanelController/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public ActionResult Create(IFormCollection collection)
    {
        try
        {
            return RedirectToAction(nameof(Index));
        }
        catch
        {
            return View();
        }
    }

    // GET: AdminPanelController/Edit/5
    public ActionResult Edit(int id)
    {
        return View();
    }

    // POST: AdminPanelController/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public ActionResult Edit(int id, IFormCollection collection)
    {
        try
        {
            return RedirectToAction(nameof(Index));
        }
        catch
        {
            return View();
        }
    }

    // GET: AdminPanelController/Delete/5
    public ActionResult Delete(int id)
    {
        return View();
    }

    // POST: AdminPanelController/Delete/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public ActionResult Delete(int id, IFormCollection collection)
    {
        try
        {
            return RedirectToAction(nameof(Index));
        }
        catch
        {
            return View();
        }
    }
}
