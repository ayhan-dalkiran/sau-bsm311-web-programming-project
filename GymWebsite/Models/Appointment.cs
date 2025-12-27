using GymWebsite.Models.Business;
using GymWebsite.Models.Staff;
using GymWebsite.Models.Users;
using Microsoft.EntityFrameworkCore; // Precision için
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GymWebsite.Models;

// Randevu Durumları
public enum AppointmentStatus { Pending, Confirmed, Cancelled, Completed }

public class Appointment {
    [Key]
    public int Id { get; set; }

    [Required(ErrorMessage = "Tarih alanı zorunludur.")]
    [Display(Name = "Randevu Tarihi")]
    public DateTime Date { get; set; }

    // --- SNAPSHOT VERİLER (Randevu anındaki değerler) ---
    // Hizmetin süresi ve fiyatı değişse bile, bu randevununki sabit kalır.

    [Display(Name = "Süre (Dakika)")]
    public int DurationInMinutes { get; set; }

    [Display(Name = "Ücret")]
    [Precision(18, 2)] // Para birimi formatı
    public decimal Price { get; set; }

    [Display(Name = "Durum")]
    public AppointmentStatus Status { get; set; } = AppointmentStatus.Pending;


    // --- İLİŞKİLER (Foreign Keys & Navigation Properties) ---

    // 1. Randevuyu Alan Üye
    public string? GymMemberId { get; set; }
    [ForeignKey("GymMemberId")]
    public virtual UserIdentityAccount? GymMember { get; set; }

    // 2. Seçilen Antrenör
    [Required]
    public int TrainerId { get; set; }
    [ForeignKey("TrainerId")]
    public virtual Trainer? Trainer { get; set; }

    // 3. Seçilen Hizmet
    [Required]
    public int ServiceId { get; set; }
    [ForeignKey("ServiceId")]
    public virtual GymService? Service { get; set; }
}