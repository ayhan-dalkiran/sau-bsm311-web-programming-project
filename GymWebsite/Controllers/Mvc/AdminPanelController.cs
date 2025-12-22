using GymWebsite.Data;
using GymWebsite.Models;
using GymWebsite.Models.Business;
using GymWebsite.Models.Staff;
using GymWebsite.Models.Users;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GymWebsite.Controllers.Mvc;

[Authorize(Roles = "Admin")]
public class AdminPanelController : Controller {

    private readonly ApplicationDbContext _context;
    private readonly UserManager<UserIdentityAccount> _userManager;
    private readonly IWebHostEnvironment _webHostEnvironment;

    // Constructor: Gerekli servisleri içeri alıyoruz (Resim yükleme için IWebHostEnvironment eklendi)
    public AdminPanelController(ApplicationDbContext context, UserManager<UserIdentityAccount> userManager, IWebHostEnvironment webHostEnvironment) {
        _context = context;
        _userManager = userManager;
        _webHostEnvironment = webHostEnvironment;
    }

    // GET: AdminPanel/Index
    public ActionResult Index() => View();

    // ==========================================
    // 1. SALON VE HİZMET YÖNETİMİ (GYM)
    // ==========================================

    public ActionResult Gym() {
        var gym = _context.Gyms.Include(g => g.Services).FirstOrDefault();

        // Eğer veritabanı boşsa varsayılan salonu oluştur
        if (gym == null) {
            gym = new Gym {
                Name = "Varsayılan Spor Salonu",
                Address = "Adres Giriniz",
                Email = "info@gym.com",
                PhoneNumber = "05550000000",
                WeekdayHours = "09:00 - 22:00",
                WeekendHours = "10:00 - 20:00"
            };
            _context.Gyms.Add(gym);
            _context.SaveChanges();
        }
        return View(gym);
    }

    // Salon Bilgilerini Güncelleme (POST)
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> EditGym(int id, Gym model) {
        if (id != model.Id) return NotFound();

        // Model validasyonu (İlişkisel alanları yoksayarak)
        ModelState.Remove("Services");
        ModelState.Remove("Trainers");
        ModelState.Remove("OpenDayTimeSpans");

        if (ModelState.IsValid) {
            try {
                var existingGym = await _context.Gyms.FindAsync(id);
                if (existingGym == null) return NotFound();

                // Alanları güncelle
                existingGym.Name = model.Name;
                existingGym.Address = model.Address;
                existingGym.PhoneNumber = model.PhoneNumber;
                existingGym.Email = model.Email;
                existingGym.WeekdayHours = model.WeekdayHours;
                existingGym.WeekendHours = model.WeekendHours;

                _context.Update(existingGym);
                await _context.SaveChangesAsync();

                TempData["SuccessMessage"] = "Salon bilgileri güncellendi!";
            }
            catch (DbUpdateConcurrencyException) {
                if (!_context.Gyms.Any(e => e.Id == model.Id)) return NotFound();
                else throw;
            }
            return RedirectToAction(nameof(Gym));
        }
        return View("Gym", model);
    }

    // Hizmet Ekleme Sayfası (GET)
    public ActionResult AddGymService() => View();

    // Hizmet Ekleme İşlemi (POST)
    [HttpPost]
    [ValidateAntiForgeryToken]
    public ActionResult AddGymService(GymService model) {
        var gym = _context.Gyms.FirstOrDefault();
        if (gym != null) {
            model.Gym = gym;
            _context.GymServices.Add(model);
            _context.SaveChanges();
            TempData["SuccessMessage"] = "Hizmet eklendi.";
        }
        else {
            TempData["ErrorMessage"] = "Salon bulunamadı.";
        }
        return RedirectToAction(nameof(Gym));
    }

    // Hizmet Silme İşlemi
    public ActionResult DeleteGymService(int id) {
        var gymService = _context.GymServices.Find(id);
        if (gymService != null) {
            _context.GymServices.Remove(gymService);
            _context.SaveChanges();
            TempData["ErrorMessage"] = "Hizmet silindi.";
        }
        return RedirectToAction(nameof(Gym));
    }


    // ==========================================
    // 2. KULLANICI YÖNETİMİ (USERS)
    // ==========================================

    public ActionResult Users() {
        return View(_context.Users.OrderByDescending(u => u.RegistrationDateTime).ToList());
    }

    public ActionResult CreateUser() => View();

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> CreateUser(UserIdentityAccount model, string Password) {
        if (!ModelState.IsValid) return View(model);

        model.RegistrationDateTime = DateTime.Now;
        model.EmailConfirmed = true;
        model.UserName = model.Email;

        var result = await _userManager.CreateAsync(model, Password);

        if (result.Succeeded) {
            await _userManager.AddToRoleAsync(model, "Member");
            TempData["SuccessMessage"] = "Kullanıcı oluşturuldu.";
            return RedirectToAction(nameof(Users));
        }

        foreach (var error in result.Errors) ModelState.AddModelError("", error.Description);
        return View(model);
    }

    public ActionResult EditUser(string id) {
        var user = _context.Users.Find(id);
        return user == null ? NotFound() : View(user);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> EditUser(UserIdentityAccount model) {
        var user = await _context.Users.FindAsync(model.Id);
        if (user != null) {
            user.FirstName = model.FirstName;
            user.LastName = model.LastName;
            user.Email = model.Email;
            user.UserName = model.Email;
            user.PhoneNumber = model.PhoneNumber;

            await _context.SaveChangesAsync();
            TempData["SuccessMessage"] = "Kullanıcı güncellendi.";
        }
        return RedirectToAction(nameof(Users));
    }

    public ActionResult DeleteUser(string id) {
        var user = _context.Users.Find(id);
        return View(user);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteUser(string id, IFormCollection collection) {
        var user = await _context.Users.FindAsync(id);
        if (user != null) {
            await _userManager.DeleteAsync(user);
            TempData["ErrorMessage"] = "Kullanıcı silindi.";
        }
        return RedirectToAction(nameof(Users));
    }


    // ==========================================
    // 3. ANTRENÖR YÖNETİMİ (TRAINERS)
    // ==========================================

    public ActionResult Trainers() {
        var trainers = _context.Trainers.Include(t => t.Services).ToList();
        return View(trainers);
    }

    public ActionResult Create() {
        ViewBag.Services = _context.GymServices.ToList();
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(Trainer trainer, IFormFile? ImageFile, List<int> SelectedServiceIds) {

        // Resim Yükleme
        if (ImageFile != null) {
            var extension = Path.GetExtension(ImageFile.FileName);
            var imageName = $"{Guid.NewGuid()}{extension}";
            var folderPath = Path.Combine(_webHostEnvironment.WebRootPath, "img", "trainers");

            if (!Directory.Exists(folderPath)) Directory.CreateDirectory(folderPath);

            var filePath = Path.Combine(folderPath, imageName);
            using (var stream = new FileStream(filePath, FileMode.Create)) {
                await ImageFile.CopyToAsync(stream);
            }
            trainer.ImageUrl = "/img/trainers/" + imageName;
        }
        else {
            trainer.ImageUrl = "https://ui-avatars.com/api/?name=" + trainer.FirstName + "+" + trainer.LastName;
        }

        // Hizmet Eşleştirme
        if (SelectedServiceIds != null && SelectedServiceIds.Any()) {
            trainer.Services = _context.GymServices.Where(s => SelectedServiceIds.Contains(s.Id)).ToList();
        }

        // Salon Bağlama
        var defaultGym = _context.Gyms.FirstOrDefault();
        if (defaultGym != null) trainer.Gym = defaultGym;

        _context.Trainers.Add(trainer);
        await _context.SaveChangesAsync();

        TempData["SuccessMessage"] = "Antrenör eklendi.";
        return RedirectToAction(nameof(Trainers));
    }

    public ActionResult Edit(int id) {
        var trainer = _context.Trainers.Include(t => t.Services).FirstOrDefault(t => t.Id == id);
        if (trainer == null) return NotFound();
        ViewBag.Services = _context.GymServices.ToList();
        return View(trainer);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, Trainer model, IFormFile? ImageFile, List<int> SelectedServiceIds) {
        var trainerToUpdate = _context.Trainers.Include(t => t.Services).FirstOrDefault(t => t.Id == id);

        if (trainerToUpdate != null) {
            trainerToUpdate.FirstName = model.FirstName;
            trainerToUpdate.LastName = model.LastName;
            trainerToUpdate.Bio = model.Bio;
            trainerToUpdate.WorkStartHour = model.WorkStartHour;
            trainerToUpdate.WorkEndHour = model.WorkEndHour;

            if (ImageFile != null) {
                var extension = Path.GetExtension(ImageFile.FileName);
                var imageName = $"{Guid.NewGuid()}{extension}";
                var path = Path.Combine(_webHostEnvironment.WebRootPath, "img", "trainers", imageName);

                using (var stream = new FileStream(path, FileMode.Create)) {
                    await ImageFile.CopyToAsync(stream);
                }
                trainerToUpdate.ImageUrl = "/img/trainers/" + imageName;
            }

            // Hizmetleri Güncelle
            trainerToUpdate.Services.Clear();
            if (SelectedServiceIds != null) {
                var services = _context.GymServices.Where(s => SelectedServiceIds.Contains(s.Id)).ToList();
                foreach (var service in services) trainerToUpdate.Services.Add(service);
            }

            await _context.SaveChangesAsync();
            TempData["SuccessMessage"] = "Antrenör güncellendi.";
            return RedirectToAction(nameof(Trainers));
        }
        return View(model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public ActionResult Delete(int id) {
        var trainer = _context.Trainers.Include(t => t.Services).FirstOrDefault(t => t.Id == id);
        if (trainer != null) {
            trainer.Services.Clear();
            _context.Trainers.Remove(trainer);
            _context.SaveChanges();
            TempData["ErrorMessage"] = "Antrenör silindi.";
        }
        return RedirectToAction(nameof(Trainers));
    }


    // ==========================================
    // 4. RANDEVU YÖNETİMİ (APPOINTMENTS)
    // ==========================================

    public async Task<IActionResult> Appointments() {
        var appointments = await _context.Appointments
                                         .Include(a => a.GymMember)
                                         .Include(a => a.Trainer)
                                         .Include(a => a.Service)
                                         .OrderByDescending(a => a.Date)
                                         .ToListAsync();
        return View(appointments);
    }

    public async Task<IActionResult> ApproveAppointment(int id) {
        var appointment = await _context.Appointments.FindAsync(id);
        if (appointment != null) {
            appointment.Status = AppointmentStatus.Confirmed;
            await _context.SaveChangesAsync();
            TempData["SuccessMessage"] = "Randevu onaylandı.";
        }
        return RedirectToAction(nameof(Appointments));
    }

    public async Task<IActionResult> CancelAppointment(int id) {
        var appointment = await _context.Appointments.FindAsync(id);
        if (appointment != null) {
            appointment.Status = AppointmentStatus.Cancelled;
            await _context.SaveChangesAsync();
            TempData["ErrorMessage"] = "Randevu iptal edildi.";
        }
        return RedirectToAction(nameof(Appointments));
    }

    public async Task<IActionResult> DeleteAppointment(int id) {
        var appointment = await _context.Appointments
                                        .Include(a => a.GymMember)
                                        .Include(a => a.Trainer)
                                        .Include(a => a.Service)
                                        .FirstOrDefaultAsync(a => a.Id == id);
        return appointment == null ? NotFound() : View(appointment);
    }

    [HttpPost, ActionName("DeleteAppointment")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteAppointmentConfirmed(int id) {
        var appointment = await _context.Appointments.FindAsync(id);
        if (appointment != null) {
            _context.Appointments.Remove(appointment);
            await _context.SaveChangesAsync();
            TempData["ErrorMessage"] = "Randevu silindi.";
        }
        return RedirectToAction(nameof(Appointments));
    }
}