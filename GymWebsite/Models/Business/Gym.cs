using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using GymWebsite.Models.Staff; // Trainer ilişkisi için gerekebilir
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GymWebsite.Models.Business;

[Index(nameof(Name), IsUnique = true)]
public class Gym {
    [Key]
    [HiddenInput]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
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

    [Required(ErrorMessage = "E-posta alanı zorunludur.")]
    [MaxLength(320, ErrorMessage = "E-posta 320 karakterden fazla olamaz.")]
    [EmailAddress(ErrorMessage = "Geçerli bir e-posta adresi giriniz.")]
    [Display(Name = "E-posta")]
    public string? Email { get; set; } = string.Empty;

    // --- YENİ EKLENEN ÇALIŞMA SAATLERİ ---
    // Admin panelinden metin olarak (Örn: "09:00 - 22:00") girilecek.

    [Display(Name = "Hafta İçi Çalışma Saatleri")]
    [MaxLength(50, ErrorMessage = "Çok uzun bir değer girdiniz.")]
    public string WeekdayHours { get; set; } = "09:00 - 22:00";

    [Display(Name = "Hafta Sonu Çalışma Saatleri")]
    [MaxLength(50, ErrorMessage = "Çok uzun bir değer girdiniz.")]
    public string WeekendHours { get; set; } = "10:00 - 20:00";
    // -------------------------------------

    // İlişkiler
    public virtual ICollection<GymOpenDayTimeSpan> OpenDayTimeSpans { get; set; } = new List<GymOpenDayTimeSpan>();
    public virtual ICollection<GymService> Services { get; set; } = new List<GymService>();
    public virtual ICollection<Trainer> Trainers { get; set; } = new List<Trainer>();
}