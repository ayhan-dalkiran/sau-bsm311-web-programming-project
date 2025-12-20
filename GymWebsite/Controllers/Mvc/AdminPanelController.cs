using GymWebsite.Models;           
using GymWebsite.Models.Users;     
using GymWebsite.Models.Staff;     
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;  

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
        var userList = new List<GymWebsite.Models.Users.GymUserAccount>
        {
            new GymWebsite.Models.Users.GymUserAccount { Id = "user-1", FirstName = "Ahmet", LastName = "Yılmaz", Email = "ahmet@mail.com", PhoneNumber = "05551112233", RegistrationDateTime = DateTime.Now.AddDays(-10) },
            new GymWebsite.Models.Users.GymUserAccount { Id = "user-2", FirstName = "Zeynep", LastName = "Demir", Email = "zeynep@mail.com", PhoneNumber = "05554445566", RegistrationDateTime = DateTime.Now.AddDays(-5) },
            new GymWebsite.Models.Users.GymUserAccount { Id = "user-3", FirstName = "Mehmet", LastName = "Kaya", Email = "mehmet@mail.com", PhoneNumber = "05329998877", RegistrationDateTime = DateTime.Now }
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
    public ActionResult CreateUser(GymWebsite.Models.Users.GymUserAccount model, string Password) {
        // Not: Gerçek backend'de burada UserManager.CreateAsync kullanılacak.
        // Şimdilik listeye dönüyoruz.
        return RedirectToAction(nameof(Users));
    }

    // Üye Düzenleme (GET)
    public ActionResult EditUser(string id) {
        // Test verisi
        var fakeUser = new GymWebsite.Models.Users.GymUserAccount {
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
    public ActionResult EditUser(GymWebsite.Models.Users.GymUserAccount model) {
        return RedirectToAction(nameof(Users));
    }

    // Üye Silme (GET)
    public ActionResult DeleteUser(string id) {
        var fakeUser = new GymWebsite.Models.Users.GymUserAccount {
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
        // Test için sahte bir randevu listesi oluşturuyoruz.
        // Modelindeki "required" alanları doldurmak zorundayız.
        var appointments = new List<Appointment>
        {
            new Appointment
            {
                Id = 1,
                Date = DateTime.Now.AddDays(1).AddHours(14), // Yarın 14:00
                DurationInMinutes = 60,
                Price = 500.00m,
                Status = AppointmentStatus.Pending, // Bekliyor
                
                // Modelinde isim "GymMember" olduğu için burası önemli!
                GymMember = new GymUserAccount
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
                Date = DateTime.Now.AddHours(2), // Bugün 2 saat sonra
                DurationInMinutes = 45,
                Price = 350.50m,
                Status = AppointmentStatus.Confirmed, // Onaylandı
                
                GymMember = new GymUserAccount
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

    // 1. Randevu Onaylama (Yeşil Tık)
    public ActionResult ApproveAppointment(int id) {
        // Backend Kodu: var app = _context.Appointments.Find(id); app.Status = AppointmentStatus.Confirmed; _context.SaveChanges();

        TempData["SuccessMessage"] = $"{id} numaralı randevu ONAYLANDI ve üyeye bildirim gönderildi.";
        return RedirectToAction(nameof(Appointments));
    }

    // 2. Randevu İptal Etme (Kırmızı Çarpı)
    public ActionResult CancelAppointment(int id) {
        // Backend Kodu: var app = _context.Appointments.Find(id); app.Status = AppointmentStatus.Cancelled; _context.SaveChanges();

        TempData["ErrorMessage"] = $"{id} numaralı randevu İPTAL EDİLDİ.";
        return RedirectToAction(nameof(Appointments));
    }

    // 3. Randevu Silme Ekranı (GET) - Veritabanından tamamen silmek için
    public ActionResult DeleteAppointment(int id) {
        // Silme onayı için sahte veri
        var fakeApp = new GymWebsite.Models.Appointment {
            Id = id,
            Date = DateTime.Now,
            GymMember = new GymWebsite.Models.Users.GymUserAccount { FirstName = "Silinecek", LastName = "Randevu" },
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
