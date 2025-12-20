using GymWebsite.Models;
using GymWebsite.Models.Users;
using GymWebsite.Models.Staff;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace GymWebsite.Controllers.Mvc;

//[Authorize(Roles = "Admin")] 
public class AdminPanelController : Controller {
    // GET: AdminPanelController
    public ActionResult Index() => View();

    public ActionResult Gyms() {
        // Test için sahte bir salon verisi
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
            TempData["SuccessMessage"] = "Salon bilgileri başarıyla güncellendi!";
            return RedirectToAction("Gyms");
        }
        return View("Gyms", model);
    }

    public ActionResult Users() {
        // BURADA ZATEN UserIdentityAccount KULLANILMIŞ, DOĞRU OLANI BU
        var userList = new List<UserIdentityAccount>
        {
            new UserIdentityAccount { Id = "user-1", FirstName = "Ahmet", LastName = "Yılmaz", Email = "ahmet@mail.com", PhoneNumber = "05551112233", RegistrationDateTime = DateTime.Now.AddDays(-10) },
            new UserIdentityAccount { Id = "user-2", FirstName = "Zeynep", LastName = "Demir", Email = "zeynep@mail.com", PhoneNumber = "05554445566", RegistrationDateTime = DateTime.Now.AddDays(-5) },
            new UserIdentityAccount { Id = "user-3", FirstName = "Mehmet", LastName = "Kaya", Email = "mehmet@mail.com", PhoneNumber = "05329998877", RegistrationDateTime = DateTime.Now }
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
    // BURAYI GÜNCELLEDİM: GymUserAccount -> UserIdentityAccount
    public ActionResult CreateUser(UserIdentityAccount model, string Password) {
        return RedirectToAction(nameof(Users));
    }

    // Üye Düzenleme (GET)
    public ActionResult EditUser(string id) {
        // BURAYI GÜNCELLEDİM: GymUserAccount -> UserIdentityAccount
        var fakeUser = new UserIdentityAccount {
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
    // BURAYI GÜNCELLEDİM: GymUserAccount -> UserIdentityAccount
    public ActionResult EditUser(UserIdentityAccount model) {
        return RedirectToAction(nameof(Users));
    }

    // Üye Silme (GET)
    public ActionResult DeleteUser(string id) {
        // BURAYI GÜNCELLEDİM: GymUserAccount -> UserIdentityAccount
        var fakeUser = new UserIdentityAccount {
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

    public ActionResult Appointments() {
        var appointments = new List<Appointment>
        {
            new Appointment
            {
                Id = 1,
                Date = DateTime.Now.AddDays(1).AddHours(14),
                DurationInMinutes = 60,
                Price = 500.00m,
                Status = AppointmentStatus.Pending, 
                
                // HATA BURADAYDI: GymUserAccount -> UserIdentityAccount OLARAK DEĞİŞTİRDİM
                GymMember = new UserIdentityAccount
                {
                    FirstName = "Ahmet",
                    LastName = "Yılmaz",
                    Email = "ahmet@mail.com"
                },
                Trainer = new Trainer
                {
                    FirstName = "Ali",
                    LastName = "Veli"
                }
            },
            new Appointment
            {
                Id = 2,
                Date = DateTime.Now.AddHours(2),
                DurationInMinutes = 45,
                Price = 350.50m,
                Status = AppointmentStatus.Confirmed, 
                
                // HATA BURADAYDI: GymUserAccount -> UserIdentityAccount OLARAK DEĞİŞTİRDİM
                GymMember = new UserIdentityAccount
                {
                    FirstName = "Zeynep",
                    LastName = "Demir",
                    Email = "zeynep@mail.com"
                },
                Trainer = new Trainer
                {
                    FirstName = "Ayşe",
                    LastName = "Yılmaz"
                }
            }
        };

        return View(appointments);
    }

    // 1. Randevu Onaylama
    public ActionResult ApproveAppointment(int id) {
        TempData["SuccessMessage"] = $"{id} numaralı randevu ONAYLANDI ve üyeye bildirim gönderildi.";
        return RedirectToAction(nameof(Appointments));
    }

    // 2. Randevu İptal Etme
    public ActionResult CancelAppointment(int id) {
        TempData["ErrorMessage"] = $"{id} numaralı randevu İPTAL EDİLDİ.";
        return RedirectToAction(nameof(Appointments));
    }

    // 3. Randevu Silme Ekranı (GET)
    public ActionResult DeleteAppointment(int id) {
        var fakeApp = new GymWebsite.Models.Appointment {
            Id = id,
            Date = DateTime.Now,
            // HATA BURADAYDI: GymUserAccount -> UserIdentityAccount OLARAK DEĞİŞTİRDİM
            GymMember = new UserIdentityAccount { FirstName = "Silinecek", LastName = "Randevu" },
            Trainer = new GymWebsite.Models.Staff.Trainer { FirstName = "Örnek", LastName = "Hoca" }
        };
        return View(fakeApp);
    }

    // 4. Randevu Silme İşlemi (POST)
    [HttpPost]
    [ValidateAntiForgeryToken]
    public ActionResult DeleteAppointment(GymWebsite.Models.Appointment model) {
        TempData["ErrorMessage"] = "Randevu kaydı veritabanından kalıcı olarak silindi.";
        return RedirectToAction(nameof(Appointments));
    }

    // YARDIMCI METOT: Salonun sunduğu hizmetleri getirir
    // İleride burası: _context.GymServices.Where(x => x.GymId == 1).ToList(); olacak.
    private List<string> GetGymServices() {
        return new List<string>
        {
            "Bireysel Antrenman (PT)",
            "Online Uzaktan Eğitim",
            "Grup Pilates Dersi",
            "Beslenme Danışmanlığı",
            "Yarışma Hazırlık Koçluğu",
            "Fizik Tedavi & Rehabilitasyon",
            "Kick Boks Özel Ders"
        };
    }

    // GET: AdminPanelController/Details/5
    public ActionResult Details(int id) {
        return View();
    }

    // GET: AdminPanelController/Create
    public ActionResult Create() {
        // View'a hizmet listesini gönderiyoruz
        ViewBag.Services = GetGymServices();
        return View();
    }

    // POST: AdminPanelController/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    // Parametreye 'List<string> SelectedServices' eklendi
    public ActionResult Create(Trainer trainer, IFormFile ImageFile, List<string> SelectedServices) {
        try {
            // Backend Notu: Burada gelen SelectedServices listesi, Trainer.Services tablosuna kaydedilecek.
            return RedirectToAction(nameof(Trainers)); // Index yerine Trainers'a dönmesi daha mantıklı
        }
        catch {
            ViewBag.Services = GetGymServices(); // Hata olursa listeyi tekrar doldur
            return View();
        }
    }

    // GET: AdminPanelController/Edit/5
    public ActionResult Edit(int id) {
        // Hizmetleri View'a taşı
        ViewBag.Services = GetGymServices();

        var fakeTrainer = new Trainer {
            Id = id,
            FirstName = "Test",
            LastName = "Antrenör",
            Bio = "Bu, düzenleme sayfasını test etmek için oluşturulmuş geçici bir veridir.",
        };

        return View(fakeTrainer);
    }

    // POST: AdminPanelController/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public ActionResult Edit(int id, IFormCollection collection) {
        try {
            // Backend Notu: Gelen SelectedServices ile veritabanını güncelle.
            return RedirectToAction(nameof(Trainers));
        }
        catch {
            ViewBag.Services = GetGymServices();
            return View();
        }
    }

    // GET: AdminPanelController/Delete/5
    public ActionResult Delete(int id) {
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
    public ActionResult Delete(int id, IFormCollection collection) {
        try {
            return RedirectToAction(nameof(Index));
        }
        catch {
            return View();
        }
    }
}