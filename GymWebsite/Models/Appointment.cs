using GymWebsite.Models.Members;
using GymWebsite.Models.Staff;

using System;
using System.ComponentModel.DataAnnotations;

namespace GymWebsite.Models {
    public class Appointment {
        public int Id { get; set; }

        [Display(Name = "Randevu Tarihi ve Saati")]
        public DateTime Date { get; set; }

        [Display(Name = "Süre (Dakika)")]
        public int DurationMinutes { get; set; } // Örn: 60 dk

        [Display(Name = "Ücret")]
        public decimal Price { get; set; }

        [Display(Name = "Durum")]
        public AppointmentStatus Status { get; set; } // Onaylandı, Bekliyor, İptal

        // --- İlişkiler (Foreign Keys) ---

        // Hangi Antrenör?
        public int TrainerId { get; set; }
        public Trainer? Trainer { get; set; }

        // Hangi Üye? (Daha önce oluşturduğun GymMember sınıfı)
        public int GymMemberId { get; set; }
        public GymMember? GymMember { get; set; }
    }

    // Randevu durumunu yönetmek için basit bir Enum
    public enum AppointmentStatus {
        Pending,   // Bekliyor
        Confirmed, // Onaylandı
        Completed, // Tamamlandı
        Cancelled  // İptal Edildi
    }
}