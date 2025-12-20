using GymWebsite.Models;
using GymWebsite.Models.Users;
using GymWebsite.Models.Staff;
using GymWebsite.Models.Business; // BU SATIR EKLENDİ (Hata Çözümü)
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using GymWebsite.Data;
using GymWebsite.Models.Business;
using Microsoft.AspNetCore.Identity;

namespace GymWebsite.Controllers.Mvc;

//[Authorize(Roles = "Admin")] 
public class AdminPanelController(ApplicationDbContext context, UserManager<UserIdentityAccount> userManager) : Controller {
    // GET: AdminPanelController
    public ActionResult Index() => View();

    public ActionResult Gym() {
        var gym = context.Gyms.First();

        // Test için sahte bir salon verisi
        var fakeGym = new GymWebsite.Models.Business.Gym {
            Id = 1,
            Name = "Power Gym Center",
            Address = "Atatürk Mah. Cumhuriyet Cad. No:123, İstanbul",
            PhoneNumber = "0212 123 45 67",
            Email = "iletisim@powergym.com",
            Services = new List<GymService>() // Listeyi başlatıyoruz ama boş
        };

        // 2. Adım: Şimdi Hizmetleri oluşturup, 'Gym' özelliğine yukarıdaki 'fakeGym'i atıyoruz.
        // Bu sayede "Gerekli 'Gym' üyesi ayarlanmalı" hatası çözülür.
        fakeGym.Services = new List<GymService>
        {
            new GymService {
                Id = 101,
                Name = "Standart Fitness Üyeliği",
                Price = 1500.00m,
                DurationInMinutes = 0,
                Description = "1 Aylık sınırsız salon kullanımı.",
                Gym = fakeGym // <-- KİLİT NOKTA BURASI
            },
            new GymService {
                Id = 102,
                Name = "Reformer Pilates (Tek Ders)",
                Price = 450.00m,
                DurationInMinutes = 50,
                Description = "Eğitmen eşliğinde birebir ders.",
                Gym = fakeGym // <-- KİLİT NOKTA BURASI
            },
            new GymService {
                Id = 103,
                Name = "Kick Boks Özel Ders",
                Price = 600.00m,
                DurationInMinutes = 60,
                Description = "Bireysel antrenman ve teknik geliştirme.",
                Gym = fakeGym // <-- KİLİT NOKTA BURASI
            }
        };

        return View(gym);
    }

    // Salon Bilgilerini Güncelleme (POST)
    [HttpPost]
    [ValidateAntiForgeryToken]
    public ActionResult UpdateGym(Gym model) {
        if (ModelState.IsValid) {
            context.Gyms.Update(model);
            context.SaveChanges();

            TempData["SuccessMessage"] = "Salon bilgileri başarıyla güncellendi!";
            return RedirectToAction("Gym");
        }

        return View("Gym");
    }

    // YENİ: Hizmet Ekleme Sayfası (GET)
    public ActionResult AddGymService() {
        return View();
    }

    // YENİ: Hizmet Ekleme İşlemi (POST)
    [HttpPost]
    [ValidateAntiForgeryToken]
    public ActionResult AddGymService(GymService model) {
        // Not: Gerçekte burada _context.GymServices.Add(model) çalışacak.

        TempData["SuccessMessage"] = $"'{model.Name}' hizmeti başarıyla sisteme eklendi.";
        return RedirectToAction(nameof(Gym));
    }

    // YENİ: Hizmet Silme İşlemi
    public ActionResult DeleteGymService(int id) {
        // Not: Gerçekte burada _context.GymServices.Remove(...) çalışacak.
        TempData["ErrorMessage"] = "Hizmet kaydı silindi.";
        return RedirectToAction(nameof(Gym));
    }

    // ==========================================
    // ÜYE YÖNETİMİ (USERS)
    // ==========================================
    public ActionResult Users() {
        var userList = new List<UserIdentityAccount>
        {
            new UserIdentityAccount { Id = "user-1", FirstName = "Ahmet", LastName = "Yılmaz", Email = "ahmet@mail.com", PhoneNumber = "05551112233", RegistrationDateTime = DateTime.Now.AddDays(-10) },
            new UserIdentityAccount { Id = "user-2", FirstName = "Zeynep", LastName = "Demir", Email = "zeynep@mail.com", PhoneNumber = "05554445566", RegistrationDateTime = DateTime.Now.AddDays(-5) },
            new UserIdentityAccount { Id = "user-3", FirstName = "Mehmet", LastName = "Kaya", Email = "mehmet@mail.com", PhoneNumber = "05329998877", RegistrationDateTime = DateTime.Now }
        };

        return View(context.Users.ToList());
    }

    public ActionResult CreateUser() => View();

    [HttpPost]
    [ValidateAntiForgeryToken]
    // BURAYI GÜNCELLEDİM: GymUserAccount -> UserIdentityAccount
    public ActionResult CreateUser(UserIdentityAccount model, string Password) {
        if (!ModelState.IsValid) {
            return View(model);
        }

        userManager.CreateAsync(model, Password).Wait();
        return RedirectToAction(nameof(Users));
    }

    public ActionResult EditUser(string id) {
        var fakeUser = new UserIdentityAccount {
            Id = id,
            FirstName = "Test",
            LastName = "Kullanıcı",
            Email = "test@kullanici.com",
            PhoneNumber = "0555 123 45 67"
        };
        var user = context.Users.Find(id);
        return View(user!);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    // BURAYI GÜNCELLEDİM: GymUserAccount -> UserIdentityAccount
    public ActionResult EditUser(UserIdentityAccount model) {
        userManager.UpdateAsync(model).Wait();

        return RedirectToAction(nameof(Users));
    }

    public ActionResult DeleteUser(string id) {
        // BURAYI GÜNCELLEDİM: GymUserAccount -> UserIdentityAccount
        var fakeUser = new UserIdentityAccount {
            Id = id,
            FirstName = "Silinecek",
            LastName = "Üye",
            Email = "sil@mail.com"
        };
        var user = context.Users.Find(id);
        return View(user);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public ActionResult DeleteUser(string id, IFormCollection collection) {
        var user = context.Users.Find(id);
        if (user != null) {
            userManager.DeleteAsync(user).Wait();
        }
        return RedirectToAction(nameof(Users));
    }


    // ==========================================
    // ANTRENÖR YÖNETİMİ (TRAINERS)
    // ==========================================
    public ActionResult Trainers() => View();

    // Antrenör Ekleme Sayfası (GET)
    public ActionResult Create() {
        // Antrenör eklerken salonun hizmetlerini dropdown olarak gönderiyoruz
        ViewBag.Services = GetGymServices();
        return View();
    }

    // Antrenör Ekleme İşlemi (POST)
    [HttpPost]
    [ValidateAntiForgeryToken]
    public ActionResult Create(Trainer trainer, IFormFile ImageFile, List<string> SelectedServices) {
        try {
            return RedirectToAction(nameof(Trainers));
        }
        catch {
            ViewBag.Services = GetGymServices();
            return View();
        }
    }

    public ActionResult Edit(int id) {
        ViewBag.Services = GetGymServices();
        var fakeTrainer = new Trainer { Id = id, FirstName = "Test", LastName = "Antrenör", Bio = "Düzenleme testi." };
        return View(fakeTrainer);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public ActionResult Edit(int id, IFormCollection collection) {
        try { return RedirectToAction(nameof(Trainers)); }
        catch { ViewBag.Services = GetGymServices(); return View(); }
    }

    public ActionResult Delete(int id) {
        var fakeTrainer = new Trainer { Id = id, FirstName = "Silinecek", LastName = "Kişi" };
        return View(fakeTrainer);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public ActionResult Delete(int id, IFormCollection collection) => RedirectToAction(nameof(Trainers));

    // Antrenörlere atanacak hizmet isimleri listesi (Basit liste)
    private List<string> GetGymServices() {
        return new List<string> { "Bireysel Antrenman (PT)", "Online Uzaktan Eğitim", "Grup Pilates", "Beslenme Danışmanlığı", "Kick Boks" };
    }


    // ==========================================
    // RANDEVU YÖNETİMİ (APPOINTMENTS)
    // ==========================================
    public ActionResult Appointments() {
        var appointments = new List<Appointment>
        {
            new Appointment {
                Id = 1, Date = DateTime.Now.AddDays(1).AddHours(14), DurationInMinutes = 60, Price = 500.00m, Status = AppointmentStatus.Pending,
                GymMember = new UserIdentityAccount { FirstName = "Ahmet", LastName = "Yılmaz", Email = "ahmet@mail.com" },
                Trainer = new Trainer { FirstName = "Ali", LastName = "Veli" }
            },
            new Appointment {
                Id = 2, Date = DateTime.Now.AddHours(2), DurationInMinutes = 45, Price = 350.50m, Status = AppointmentStatus.Confirmed,
                GymMember = new UserIdentityAccount { FirstName = "Zeynep", LastName = "Demir", Email = "zeynep@mail.com" },
                Trainer = new Trainer { FirstName = "Ayşe", LastName = "Yılmaz" }
            }
        };
        return View(appointments);
    }

    public ActionResult ApproveAppointment(int id) {
        TempData["SuccessMessage"] = $"{id} numaralı randevu ONAYLANDI.";
        return RedirectToAction(nameof(Appointments));
    }

    public ActionResult CancelAppointment(int id) {
        TempData["ErrorMessage"] = $"{id} numaralı randevu İPTAL EDİLDİ.";
        return RedirectToAction(nameof(Appointments));
    }

    public ActionResult DeleteAppointment(int id) {
        var fakeApp = new Appointment {
            Id = id,
            Date = DateTime.Now,
            GymMember = new UserIdentityAccount { FirstName = "Silinecek", LastName = "Randevu" },
            Trainer = new Trainer { FirstName = "Örnek", LastName = "Hoca" }
        };
        return View(fakeApp);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public ActionResult DeleteAppointment(Appointment model) {
        TempData["ErrorMessage"] = "Randevu silindi.";
        return RedirectToAction(nameof(Appointments));
    }
}