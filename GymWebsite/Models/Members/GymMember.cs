using System;
using System.ComponentModel.DataAnnotations;

namespace GymWebsite.Models.Members {
    public class GymMember {
        
        public int Id { get; set; }

        [Required(ErrorMessage = "Ad alanı zorunludur.")]
        [Display(Name = "Ad")]
        public string FirstName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Soyad alanı zorunludur.")]
        [Display(Name = "Soyad")]
        public string LastName { get; set; } = string.Empty;

        [Required(ErrorMessage = "E-posta alanı zorunludur.")]
        [EmailAddress(ErrorMessage = "Geçerli bir e-posta adresi giriniz.")]
        public string Email { get; set; } = string.Empty;

        [Display(Name = "Telefon Numarası")]
        public string? PhoneNumber { get; set; }

        [Display(Name = "Kayıt Tarihi")]
        [DataType(DataType.Date)]
        public DateTime EnrollmentDate { get; set; }

        [Display(Name = "Üyelik Aktif mi?")]
        public bool IsActive { get; set; }

        // Veritabanında tutulmayan ama kod içinde kolaylık sağlayan bir yardımcı özellik
        public string FullName => $"{FirstName} {LastName}";
    }
}
