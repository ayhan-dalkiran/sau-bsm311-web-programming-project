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

    public ActionResult Users() {
        // Test için sahte üye listesi
        var userList = new List<GymWebsite.Models.Users.UserIdentityAccount>
        {
            new GymWebsite.Models.Users.UserIdentityAccount { Id = "user-1", FirstName = "Ahmet", LastName = "Yılmaz", Email = "ahmet@mail.com", PhoneNumber = "05551112233", RegistrationDateTime = DateTime.Now.AddDays(-10) },
            new GymWebsite.Models.Users.UserIdentityAccount { Id = "user-2", FirstName = "Zeynep", LastName = "Demir", Email = "zeynep@mail.com", PhoneNumber = "05554445566", RegistrationDateTime = DateTime.Now.AddDays(-5) },
            new GymWebsite.Models.Users.UserIdentityAccount { Id = "user-3", FirstName = "Mehmet", LastName = "Kaya", Email = "mehmet@mail.com", PhoneNumber = "05329998877", RegistrationDateTime = DateTime.Now }
        };

        return View(userList);
    }

    // Yeni Üye Ekleme (GET)
    public ActionResult CreateUser() {
        return View();
    }

    // Yeni Üye Ekleme (POST)
    [HttpPost]
    [ValidateAntiForgeryToken]
    public ActionResult CreateUser(GymWebsite.Models.Users.UserIdentityAccount model, string Password) {
        // Not: Gerçek backend'de burada UserManager.CreateAsync kullanılacak.
        // Şimdilik listeye dönüyoruz.
        return RedirectToAction(nameof(Users));
    }

    // Üye Düzenleme (GET)
    public ActionResult EditUser(string id) {
        // Test verisi
        var fakeUser = new GymWebsite.Models.Users.UserIdentityAccount {
            Id = id,
            FirstName = "Test",
            LastName = "Kullanıcı",
            Email = "test@kullanici.com",
            PhoneNumber = "0555 123 45 67"
        };
        return View(fakeUser);
    }

    // Üye Düzenleme (POST)
    [HttpPost]
    [ValidateAntiForgeryToken]
    public ActionResult EditUser(GymWebsite.Models.Users.UserIdentityAccount model) {
        return RedirectToAction(nameof(Users));
    }

    // Üye Silme (GET)
    public ActionResult DeleteUser(string id) {
        var fakeUser = new GymWebsite.Models.Users.UserIdentityAccount {
            Id = id,
            FirstName = "Silinecek",
            LastName = "Üye",
            Email = "sil@mail.com"
        };
        return View(fakeUser);
    }

    // Üye Silme (POST)
    [HttpPost]
    [ValidateAntiForgeryToken]
    public ActionResult DeleteUser(string id, IFormCollection collection) {
        return RedirectToAction(nameof(Users));
    }


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
