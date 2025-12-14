using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

using System.ComponentModel.DataAnnotations;

namespace GymWebsite.Models.Business;

public class Gym {
    [Key]
    [HiddenInput]
    public int Id { get; set; }

    [Unicode]
    [Required(ErrorMessage = "Salon adı boş olamaz.")]
    [MaxLength(100, ErrorMessage = "Salon adı 100 karakterden fazla olamaz.")]
    [Display(Name = "Salon Adı")]
    public required string Name { get; set; }

    [Unicode]
    [MaxLength(250, ErrorMessage = "Salon adresi 250 karakterden fazla olamaz.")]
    [Display(Name = "Adres")]
    public string? Address { get; set; }

    [Phone(ErrorMessage = "Geçerli bir telefon numarası giriniz.")]
    [Display(Name = "Telefon")]
    public string? PhoneNumber { get; set; }

    public ICollection<GymOpenDayTimeSpan> OpenDayTimeSpans { get; set; } = [];

    public ICollection<GymService> Services { get; set; } = [];

    /*
    // Çalışma Saatleri (Basit metin olarak tutabiliriz)
    [Display(Name = "Hafta İçi Çalışma Saatleri")]
    public string OpeningHoursWeekdays { get; set; } = "09:00 - 22:00";

    [Display(Name = "Hafta Sonu Çalışma Saatleri")]
    public string OpeningHoursWeekend { get; set; } = "10:00 - 20:00";
    */

    // Salonun sunduğu hizmetler listesi
}