using GymWebsite.Models;
using GymWebsite.Models.Users;
using GymWebsite.Models.Staff;
using GymWebsite.Models.Business;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using GymWebsite.Data;
using Microsoft.AspNetCore.Identity;

namespace GymWebsite.Controllers.Mvc;

//[Authorize(Roles = "Admin")] 
public class AdminPanelController(ApplicationDbContext context, UserManager<UserIdentityAccount> userManager) : Controller {

    // GET: AdminPanel/Index
    public ActionResult Index() => View();

    // ==========================================
    // SALON VE HİZMET YÖNETİMİ (GYM)
    // ==========================================

    // ACTION ADI: Gym (Tekil) - Layout'taki link ile uyumlu
    public ActionResult Gym() {
        // Veritabanından ilk salonu ve hizmetlerini getir
        var gym = context.Gyms
                         .Include(g => g.Services)
                         .FirstOrDefault();

        // Eğer veritabanı boşsa (ilk açılışta) hata vermesin, otomatik oluştursun
        if (gym == null) {
            gym = new Gym {
                Name = "Varsayılan Spor Salonu",
                Address = "Adres Giriniz",
                Email = "info@gym.com",
                PhoneNumber = "05550000000"
            };
            context.Gyms.Add(gym);
            context.SaveChanges();
        }

        return View(gym); // Views/AdminPanel/Gym.cshtml dosyasını açar
    }

    // Salon Bilgilerini Güncelleme (POST)
    [HttpPost]
    [ValidateAntiForgeryToken]
    public ActionResult UpdateGym(Gym model) {
        if (ModelState.IsValid) {
            context.Gyms.Update(model);
            context.SaveChanges();

            TempData["SuccessMessage"] = "Salon bilgileri başarıyla güncellendi!";
            return RedirectToAction(nameof(Gym)); // İşlem bitince Gym sayfasına dön
        }

        return View("Gym", model);
    }

    // YENİ: Hizmet Ekleme Sayfası (GET)
    public ActionResult AddGymService() {
        return View();
    }

    // YENİ: Hizmet Ekleme İşlemi (POST)
    [HttpPost]
    [ValidateAntiForgeryToken]
    public ActionResult AddGymService(GymService model) {
        var gym = context.Gyms.FirstOrDefault();

        if (gym != null) {
            // Hizmeti salona bağlıyoruz (Required hatasını çözer)
            model.Gym = gym;

            context.GymServices.Add(model);
            context.SaveChanges();

            TempData["SuccessMessage"] = $"'{model.Name}' hizmeti başarıyla sisteme eklendi.";
        }
        else {
            TempData["ErrorMessage"] = "Salon bulunamadı.";
        }

        return RedirectToAction(nameof(Gym)); // İşlem bitince Gym sayfasına dön
    }

    // YENİ: Hizmet Silme İşlemi
    public ActionResult DeleteGymService(int id) {
        var gymService = context.GymServices.Find(id);
        if (gymService != null) {
            context.GymServices.Remove(gymService);
            context.SaveChanges();
            TempData["ErrorMessage"] = "Hizmet kaydı silindi.";
        }
        return RedirectToAction(nameof(Gym)); // İşlem bitince Gym sayfasına dön
    }


    // ==========================================
    // DİĞER METOTLAR (USERS, TRAINERS vb.)
    // ==========================================

    public ActionResult Users() {
        return View(context.Users.ToList());
    }

    public ActionResult CreateUser() => View();

    [HttpPost]
    [ValidateAntiForgeryToken]
    public ActionResult CreateUser(UserIdentityAccount model, string Password) {
        if (!ModelState.IsValid) return View(model);
        userManager.CreateAsync(model, Password).Wait();
        return RedirectToAction(nameof(Users));
    }

    public ActionResult EditUser(string id) {
        var user = context.Users.Find(id);
        return View(user!);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public ActionResult EditUser(UserIdentityAccount model) {
        // Not: Gerçek uygulamada kullanıcıyı DB'den çekip alanlarını güncellemek daha sağlıklıdır.
        // Şimdilik basit tutuyoruz.
        var user = context.Users.Find(model.Id);
        if (user != null) {
            user.FirstName = model.FirstName;
            user.LastName = model.LastName;
            user.Email = model.Email;
            user.PhoneNumber = model.PhoneNumber;
            context.SaveChanges();
        }
        return RedirectToAction(nameof(Users));
    }

    public ActionResult DeleteUser(string id) {
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
    // ANTRENÖR YÖNETİMİ (TRAINERS) - GERÇEK VERİTABANI
    // ==========================================

    // LİSTELEME
    public ActionResult Trainers() {
        // Include(t => t.Services) sayesinde antrenörün hizmetlerini de getiriyoruz
        var trainers = context.Trainers
                              .Include(t => t.Services)
                              .ToList();

        return View(trainers);
    }

    // EKLEME SAYFASI (GET)
    public ActionResult Create() {
        // Veritabanındaki AKTİF hizmetleri çekip View'a gönderiyoruz
        // Böylece checkbox listesinde "Pilates", "Fitness" gibi gerçek veriler çıkacak.
        ViewBag.Services = context.GymServices.ToList();
        return View();
    }

    // EKLEME İŞLEMİ (POST)
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(Trainer trainer, IFormFile? ImageFile, List<int> SelectedServiceIds) {

        // 1. Resim Yükleme İşlemi
        if (ImageFile != null) {
            var extension = Path.GetExtension(ImageFile.FileName);
            var imageName = string.Format($"{Guid.NewGuid()}{extension}");
            var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/img/trainers", imageName);

            // Klasör yoksa oluştur
            Directory.CreateDirectory(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/img/trainers"));

            using (var stream = new FileStream(path, FileMode.Create)) {
                await ImageFile.CopyToAsync(stream);
            }
            trainer.ImageUrl = "/img/trainers/" + imageName;
        }
        else {
            // Resim yüklenmediyse varsayılan bir resim atayabiliriz
            trainer.ImageUrl = "https://ui-avatars.com/api/?name=" + trainer.FirstName + "+" + trainer.LastName;
        }

        // 2. Hizmetleri Eşleştirme (İlişki Kurma)
        if (SelectedServiceIds != null && SelectedServiceIds.Any()) {
            // Seçilen ID'lere sahip hizmetleri veritabanından bul
            var servicesToAdd = context.GymServices.Where(s => SelectedServiceIds.Contains(s.Id)).ToList();
            trainer.Services = servicesToAdd;
        }

        // 3. Kaydetme
        context.Trainers.Add(trainer);
        await context.SaveChangesAsync();

        TempData["SuccessMessage"] = "Antrenör başarıyla eklendi.";
        return RedirectToAction(nameof(Trainers));
    }

    // DÜZENLEME SAYFASI (GET)
    public ActionResult Edit(int id) {
        // Düzenlenecek antrenörü ve hizmetlerini getir
        var trainer = context.Trainers.Include(t => t.Services).FirstOrDefault(t => t.Id == id);

        if (trainer == null) return NotFound();

        // Tüm hizmetleri listelemek için (Checkbox'ları doldurmak için)
        ViewBag.Services = context.GymServices.ToList();

        return View(trainer);
    }

    // DÜZENLEME İŞLEMİ (POST)
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, Trainer model, IFormFile? ImageFile, List<int> SelectedServiceIds) {

        var trainerToUpdate = context.Trainers.Include(t => t.Services).FirstOrDefault(t => t.Id == id);

        if (trainerToUpdate != null) {
            // Bilgileri Güncelle
            trainerToUpdate.FirstName = model.FirstName;
            trainerToUpdate.LastName = model.LastName;
            trainerToUpdate.Bio = model.Bio;
            trainerToUpdate.WorkStartHour = model.WorkStartHour;
            trainerToUpdate.WorkEndHour = model.WorkEndHour;

            // Resim Güncelleme (Eğer yeni resim seçildiyse)
            if (ImageFile != null) {
                var extension = Path.GetExtension(ImageFile.FileName);
                var imageName = string.Format($"{Guid.NewGuid()}{extension}");
                var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/img/trainers", imageName);

                using (var stream = new FileStream(path, FileMode.Create)) {
                    await ImageFile.CopyToAsync(stream);
                }
                trainerToUpdate.ImageUrl = "/img/trainers/" + imageName;
            }

            // Hizmetleri Güncelleme (Çoka-Çok İlişki Yönetimi)
            // Önce mevcut ilişkileri temizle, sonra yenileri ekle (En basit yöntem)
            trainerToUpdate.Services.Clear();
            if (SelectedServiceIds != null) {
                var services = context.GymServices.Where(s => SelectedServiceIds.Contains(s.Id)).ToList();
                foreach (var service in services) {
                    trainerToUpdate.Services.Add(service);
                }
            }

            await context.SaveChangesAsync();
            TempData["SuccessMessage"] = "Antrenör bilgileri güncellendi.";
            return RedirectToAction(nameof(Trainers));
        }

        return View(model);
    }

    // SİLME İŞLEMİ (GÜNCELLENMİŞ HALİ)
    [HttpPost]
    [ValidateAntiForgeryToken]
    public ActionResult Delete(int id) {
        // 1. Antrenörü, bağlı olduğu hizmetlerle (Services) birlikte çekiyoruz
        var trainer = context.Trainers
                             .Include(t => t.Services)
                             .FirstOrDefault(t => t.Id == id);

        if (trainer != null) {
            // 2. İLİŞKİLERİ TEMİZLEME
            // Antrenör silinmeden önce, ona atanmış hizmetlerin "Trainer" bilgisini temizliyoruz.
            // Bu işlem GymServices tablosundaki TrainerId sütununu NULL yapar.
            trainer.Services.Clear();
            context.SaveChanges(); // Önce bağları kopardığımızı kaydediyoruz

            // 3. ARTIK SİLEBİLİRİZ
            context.Trainers.Remove(trainer);
            context.SaveChanges(); // Şimdi hocayı siliyoruz

            TempData["ErrorMessage"] = "Antrenör silindi.";
        }
        return RedirectToAction(nameof(Trainers));
    }

    private List<string> GetGymServices() {
        return new List<string> { "Bireysel Antrenman (PT)", "Online Uzaktan Eğitim", "Grup Pilates", "Beslenme Danışmanlığı", "Kick Boks" };
    }

    public ActionResult Appointments() {
        // Şimdilik sahte veri
        var appointments = new List<Appointment>
        {
            new Appointment {
                Id = 1, Date = DateTime.Now.AddDays(1).AddHours(14), DurationInMinutes = 60, Price = 500.00m, Status = AppointmentStatus.Pending,
                GymMember = new UserIdentityAccount { FirstName = "Ahmet", LastName = "Yılmaz", Email = "ahmet@mail.com" },
                Trainer = new Trainer { FirstName = "Ali", LastName = "Veli" }
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