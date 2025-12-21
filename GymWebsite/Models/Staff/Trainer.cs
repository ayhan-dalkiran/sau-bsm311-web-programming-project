using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

using System.ComponentModel.DataAnnotations;

namespace GymWebsite.Models.Staff; 

public class Trainer {
    [Key]
    [HiddenInput]
    public int Id { get; set; }

    [Unicode]
    [Required(ErrorMessage = "Ad alanı zorunludur.")]
    [MaxLength(100, ErrorMessage = "Eğitmen adı 100 karakterden fazla olamaz.")]
    [Display(Name = "Ad")]
    public string FirstName { get; set; } = string.Empty;

    [Unicode]
    [Required(ErrorMessage = "Soyad alanı zorunludur.")]
    [MaxLength(100, ErrorMessage = "Eğitmen soyadı 100 karakterden fazla olamaz.")]
    [Display(Name = "Soyad")]
    public string LastName { get; set; } = string.Empty;

    [Unicode]
    [MaxLength(4000, ErrorMessage = "Eğitmen biyografisi 4000 karakterden fazla olamaz.")]
    [Display(Name = "Biyografi")]
    public string? Bio { get; set; }

    // Bir antrenörün birden fazla "Müsaitlik Zamanı" olabilir.
    // (Pazartesi için ayrı, Salı için ayrı satırlar gibi)
    public ICollection<TrainerOpenDayTimeSpan> OpenDayTimeSpans { get; set; } = [];

    // Antrenörün yapabildiklerini (Uzmanlıklarını) burada ilişkilendiriyoruz.
    // Bir antrenörün birden fazla uzmanlığı olabilir.
    public ICollection<Specialty> Specialties { get; set; } = [];

    // Antrenörün randevularını görebilmesi için
    public ICollection<Appointment> Appointments { get; set; } = [];
}