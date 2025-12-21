using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GymWebsite.Models.Users;

[NotMapped]
public class UserDetails {   
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
    public string Email { get; set; } = string.Empty;

    [MaxLength(15, ErrorMessage = "Telefon numarası 15 karakterden fazla olamaz.")]
    [Phone(ErrorMessage = "Geçerli bir telefon numarası giriniz.")]
    [Display(Name = "Telefon Numarası")]
    public string PhoneNumber { get; set; } = string.Empty;

    [Display(Name = "Kayıt Tarihi")]
    public DateTime RegistrationDateTime { get; set; } = DateTime.UtcNow;

    [MinLength(3, ErrorMessage = "Parola en az 3 karakter olmalıdır.")]
    [Display(Name = "Parola")]
    public string Password { get; set; } = string.Empty;

    [Compare(nameof(Password), ErrorMessage = "")]
    [Display(Name = "Parola Terkar")]
    public string PasswordCheck { get; set; } = string.Empty;

    // Veritabanında tutulmayan ama kod içinde kolaylık sağlayan bir yardımcı özellik
    [NotMapped]
    public string FullName => $"{FirstName} {LastName}";
}
