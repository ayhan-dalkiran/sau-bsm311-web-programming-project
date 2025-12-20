using GymWebsite.Models.Staff;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GymWebsite.Controllers.Mvc;

//[Authorize(Roles = "Admin")] DENEME AMAÇLI YORUM SATIRI YAPTIM BURAYI SON AŞAMADA TEKRAR KOD YAP
public class AdminPanelController : Controller {
    // GET: AdminPanelController
    public ActionResult Index() => View();

    public ActionResult Gyms() {
        // Test için sahte bir salon verisi oluşturup sayfaya gönderiyoruz.
        // Backend bağlanınca burası veritabanından çekilecek: _context.Gyms.FirstOrDefault();
        var fakeGym = new GymWebsite.Models.Business.Gym {
            Id = 1,
            Name = "Power Gym Center",
            Address = "Atatürk Mah. Cumhuriyet Cad. No:123, İstanbul",
            PhoneNumber = "0212 123 45 67",
            Email = "iletisim@powergym.com"
        };

        return View(fakeGym);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public ActionResult UpdateGym(GymWebsite.Models.Business.Gym model) {
        if (ModelState.IsValid) {
            // Buraya arkadaşın veritabanı güncelleme kodunu yazacak.
            // Örn: _context.Gyms.Update(model); _context.SaveChanges();

            // Şimdilik başarılıymış gibi aynı sayfaya geri dönelim
            // TempData ile mesaj gösterelim (Tasarımda ekleyeceğiz)
            TempData["SuccessMessage"] = "Salon bilgileri başarıyla güncellendi!";
            return RedirectToAction("Gyms");
        }

        // Hata varsa formu tekrar göster
        return View("Gyms", model);
    }

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
    public ActionResult Create(Trainer trainer, IFormFile ImageFile)
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
    public ActionResult Edit(int id) {
        // Backend hazır olmadığı için sayfaya SAHTE bir antrenör gönderiyoruz.
        // Böylece formu doluymuş gibi test edebilirsin.
        var fakeTrainer = new Trainer {
            Id = id,
            FirstName = "Test",
            LastName = "Antrenör",
            Bio = "Bu, düzenleme sayfasını test etmek için oluşturulmuş geçici bir veridir. Backend bağlandığında buraya gerçek veritabanı verisi gelecektir.",
            // Resim veya diğer alanlar şimdilik boş kalabilir
        };

        return View(fakeTrainer);
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
    public ActionResult Delete(int id) {
        // Silme sayfası için de sahte bir veri oluşturalım
        var fakeTrainer = new Trainer {
            Id = id,
            FirstName = "Silinecek",
            LastName = "Kişi",
            Bio = "Bu kişi silinecek..."
        };

        return View(fakeTrainer);
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
