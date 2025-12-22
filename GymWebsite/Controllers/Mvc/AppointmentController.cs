using GymWebsite.Data;
using GymWebsite.Models;
using GymWebsite.Models.Users;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace GymWebsite.Controllers.Mvc;

[Authorize] // Sadece giriş yapmış üyeler
public class AppointmentController : Controller {

    private readonly ApplicationDbContext _context;
    private readonly UserManager<UserIdentityAccount> _userManager;

    // Constructor: Veritabanı ve Kullanıcı yöneticisini içeri alıyoruz
    public AppointmentController(ApplicationDbContext context, UserManager<UserIdentityAccount> userManager) {
        _context = context;
        _userManager = userManager;
    }

    // 1. Randevu Alma Sayfası (GET)
    public IActionResult Create() {
        // A. ANTRENÖRLERİ GETİR
        // Veritabanındaki antrenörleri Ad+Soyad birleştirerek listeye atıyoruz
        ViewData["Trainers"] = new SelectList(_context.Trainers.Select(t => new {
            Id = t.Id,
            FullName = t.FirstName + " " + t.LastName
        }), "Id", "FullName");

        // B. HİZMETLERİ GETİR (Sorunun Çözümü Burası)
        // Hizmetleri tam liste olarak gönderiyoruz ki View tarafında Fiyat ve Süre bilgilerini okuyabilelim.
        ViewBag.ServiceList = _context.GymServices.ToList();

        return View();
    }

    // 2. Randevu Kaydetme İşlemi (POST)
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(int TrainerId, int ServiceId, DateTime Date) {

        // Temel kontroller
        var trainer = await _context.Trainers.FindAsync(TrainerId);
        var service = await _context.GymServices.FindAsync(ServiceId);
        var currentUser = await _userManager.GetUserAsync(User);

        if (trainer == null || service == null || currentUser == null) {
            TempData["ErrorMessage"] = "Seçilen bilgilerde eksiklik var.";
            return RedirectToAction(nameof(Create));
        }

        // Tarih Geçmiş mi?
        if (Date < DateTime.Now) {
            TempData["ErrorMessage"] = "Geçmiş bir tarihe randevu alamazsınız.";
            return ReloadView();
        }

        // Çakışma Kontrolü (Conflict Check)
        DateTime newEnd = Date.AddMinutes(service.DurationInMinutes);
        bool isConflict = await _context.Appointments
            .AnyAsync(a => a.TrainerId == TrainerId
                        && a.Status != AppointmentStatus.Cancelled
                        && a.Date < newEnd
                        && a.Date.AddMinutes(a.DurationInMinutes) > Date);

        if (isConflict) {
            TempData["ErrorMessage"] = "Seçilen saatte antrenör dolu. Lütfen başka bir saat seçin.";
            return ReloadView();
        }

        // Kayıt
        var appointment = new Appointment {
            Date = Date,
            DurationInMinutes = service.DurationInMinutes,
            Price = service.Price,
            Status = AppointmentStatus.Pending,
            TrainerId = TrainerId,
            ServiceId = ServiceId,
            GymMemberId = currentUser.Id
        };

        _context.Appointments.Add(appointment);
        await _context.SaveChangesAsync();

        TempData["SuccessMessage"] = "Randevunuz başarıyla oluşturuldu!";
        return RedirectToAction("MyAppointments");
    }

    // Hata durumunda sayfayı verilerle tekrar yükleyen yardımcı metot
    private IActionResult ReloadView() {
        ViewData["Trainers"] = new SelectList(_context.Trainers.Select(t => new { Id = t.Id, FullName = t.FirstName + " " + t.LastName }), "Id", "FullName");
        ViewBag.ServiceList = _context.GymServices.ToList();
        return View("Create");
    }

    // Randevularım Sayfası
    public async Task<IActionResult> MyAppointments() {
        var user = await _userManager.GetUserAsync(User);
        if (user == null) return RedirectToAction("Login", "User");

        var list = await _context.Appointments
            .Include(a => a.Trainer)
            .Include(a => a.Service)
            .Where(a => a.GymMemberId == user.Id)
            .OrderByDescending(a => a.Date)
            .ToListAsync();

        return View(list);
    }
}