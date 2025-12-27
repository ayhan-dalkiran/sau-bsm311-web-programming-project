using System.ComponentModel.DataAnnotations;

namespace GymWebsite.Models.Users;

// 1. Profil Düzenleme Modeli
public class UserEditViewModel {
    [Display(Name = "Ad")]
    [Required(ErrorMessage = "Ad alanı zorunludur.")]
    public string FirstName { get; set; } = string.Empty;

    [Display(Name = "Soyad")]
    [Required(ErrorMessage = "Soyad alanı zorunludur.")]
    public string LastName { get; set; } = string.Empty;

    [Display(Name = "Telefon Numarası")]
    [Phone(ErrorMessage = "Geçerli bir telefon numarası giriniz.")]
    public string? PhoneNumber { get; set; }
}

// 2. Şifre Değiştirme Modeli
public class ChangePasswordViewModel {
    [Required(ErrorMessage = "Mevcut şifrenizi girmeniz gerekmektedir.")]
    [DataType(DataType.Password)]
    [Display(Name = "Mevcut Şifre")]
    public string OldPassword { get; set; } = string.Empty;

    [Required(ErrorMessage = "Yeni şifre zorunludur.")]
    [StringLength(100, ErrorMessage = "{0} en az {2} karakter uzunluğunda olmalıdır.", MinimumLength = 6)]
    [DataType(DataType.Password)]
    [Display(Name = "Yeni Şifre")]
    public string NewPassword { get; set; } = string.Empty;

    [DataType(DataType.Password)]
    [Display(Name = "Yeni Şifre (Tekrar)")]
    [Compare("NewPassword", ErrorMessage = "Yeni şifreler birbiriyle uyuşmuyor.")]
    public string ConfirmPassword { get; set; } = string.Empty;
}