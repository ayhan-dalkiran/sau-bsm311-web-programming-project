using GymWebsite.Models.Members;
using GymWebsite.Models.Staff;

using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

using System.ComponentModel.DataAnnotations;

namespace GymWebsite.Models; 
public class Appointment {
    [Key]
    [HiddenInput]
    public int Id { get; set; }

    public required GymMember GymMember { get; set; }

    public required Trainer Trainer { get; set; }

    [Required(ErrorMessage = "Randevu tarihi ve saati olamaz.")]
    [Display(Name = "Randevu Tarihi ve Saati")]
    public DateTime Date { get; set; }

    [Required(ErrorMessage = "Randevu süresi boş olamaz.")]
    [Range(0, int.MaxValue, ErrorMessage = "Randevu süresi negatif bir sayı olamaz.")]
    [Display(Name = "Süre (Dakika)")]
    public int DurationInMinutes { get; set; }

    [Required(ErrorMessage = "Randevu ücreti boş olamaz.")]
    [Precision(17, 2)]
    [Range(0, 15e9, ErrorMessage = "Randevu ücreti pozitif ve 15 haneden küçük olmalıdır.")]
    [Display(Name = "Ücret")]
    public decimal Price { get; set; }

    [Display(Name = "Durum")]
    public AppointmentStatus Status { get; set; } // Onaylandı, Bekliyor, İptal
}

// Randevu durumunu yönetmek için basit bir Enum
public enum AppointmentStatus {
    Pending,   // Bekliyor
    Confirmed, // Onaylandı
    Completed, // Tamamlandı
    Cancelled  // İptal Edildi
}