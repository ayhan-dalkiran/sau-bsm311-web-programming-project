using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GymWebsite.Models.Users; 

public class GymMember : ApplicationUser {
    [Unicode]
    [Required(ErrorMessage = "Ad alanı zorunludur.")]
    [MaxLength(100, ErrorMessage = "Üye adı 100 karakterden fazla olamaz.")]
    [Display(Name = "Ad")]
    public string FirstName { get; set; } = string.Empty;

    [Unicode]
    [Required(ErrorMessage = "Soyad alanı zorunludur.")]
    [MaxLength(100, ErrorMessage = "Üye soyadı 100 karakterden fazla olamaz.")]
    [Display(Name = "Soyad")]
    public string LastName { get; set; } = string.Empty;
/*
    [MaxLength(15, ErrorMessage = "Telefon numarası 15 karakterden fazla olamaz.")]
    [Phone(ErrorMessage = "Geçerli bir telefon numarası giriniz.")]
    [Display(Name = "Telefon Numarası")]
    public string? PhoneNumber { get; set; }
*/
    [Display(Name = "Kayıt Tarihi")]
    public DateTime RegistrationDateTime { get; set; }

    [Display(Name = "Kayıt Bitiş Tarihi")]
    public DateTime? DeactivationDateTime { get; set; }

    // Veritabanında tutulmayan ama kod içinde kolaylık sağlayan bir yardımcı özellik
    [NotMapped]
    public string FullName => $"{FirstName} {LastName}";
}
