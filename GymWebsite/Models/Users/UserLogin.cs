using System.ComponentModel.DataAnnotations;

namespace GymWebsite.Models.Users;

public class UserLogin {
    [Display(Name = "E-posta")]
    public string Email { get; set; } = string.Empty;
    [Display(Name = "Parola")]
    public string Password { get; set; } = string.Empty;
    [Display(Name = "Beni Hatırla")]
    public bool Persistent { get; set; } = false;
}
