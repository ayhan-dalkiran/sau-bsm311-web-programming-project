using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GymWebsite.Models.Users;

public class GymUserAccount : IdentityUser {
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

    [Required(ErrorMessage = "E-posta alanı zorunludur.")]
    [MaxLength(320, ErrorMessage = "E-posta 320 karakterden fazla olamaz.")]
    [EmailAddress(ErrorMessage = "Geçerli bir e-posta adresi giriniz.")]
    [Display(Name = "E-posta")]

    public override string Email { get; set; } = string.Empty;

    [MaxLength(15, ErrorMessage = "Telefon numarası 15 karakterden fazla olamaz.")]
    [Phone(ErrorMessage = "Geçerli bir telefon numarası giriniz.")]
    [Display(Name = "Telefon Numarası")]
    public override string PhoneNumber { get; set; } = string.Empty;

    [Display(Name = "Kayıt Tarihi")]
    public DateTime RegistrationDateTime { get; set; }

    // Veritabanında tutulmayan ama kod içinde kolaylık sağlayan bir yardımcı özellik
    [NotMapped]
    public string FullName => $"{FirstName} {LastName}";
}
