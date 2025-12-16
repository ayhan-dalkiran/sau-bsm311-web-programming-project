using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

using System.ComponentModel.DataAnnotations;

namespace GymWebsite.Models.Users;

[Index(nameof(Email), IsUnique = true)]
public class BaseAppUser {
    [Key]
    public int Id { get; set; }

    [Required(ErrorMessage = "E-posta alanı boş olamaz.")]
    [MaxLength(254, ErrorMessage = "E-posta adresi 254 karakterden uzun olamaz.")]
    [EmailAddress(ErrorMessage = "Geçersiz e-posta adresi.")]
    [Display(Name = "E-posta")]
    public required string Email { get; set; }

    public bool IsAdmin { get; set; } = false;
}
