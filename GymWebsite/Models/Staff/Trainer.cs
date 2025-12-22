using GymWebsite.Models.Business; // GymService için gerekli
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GymWebsite.Models.Staff;

public class Trainer {
    [Key]
    public int Id { get; set; }

    [Required(ErrorMessage = "Ad alanı zorunludur.")]
    [Display(Name = "Ad")]
    public string FirstName { get; set; } = string.Empty;

    [Required(ErrorMessage = "Soyad alanı zorunludur.")]
    [Display(Name = "Soyad")]
    public string LastName { get; set; } = string.Empty;

    [Display(Name = "Biyografi")]
    public string? Bio { get; set; }

    [Display(Name = "Profil Fotoğrafı")]
    public string? ImageUrl { get; set; } // Resim yolunu tutacak

    // --- YENİ EKLENEN KISIMLAR ---

    // 1. Çalışma Saatleri (Basit başlangıç ve bitiş saati, örn: 09 ve 17)
    [Display(Name = "Mesai Başlangıç Saati")]
    public int WorkStartHour { get; set; } = 9; // Varsayılan 09:00

    [Display(Name = "Mesai Bitiş Saati")]
    public int WorkEndHour { get; set; } = 18; // Varsayılan 18:00

    // 2. Antrenörün verebildiği hizmetler (Çoka-Çok İlişki)
    // Bu liste sayesinde bir hoca birden fazla hizmet (Pilates, PT) verebilir.
    public virtual ICollection<GymService> Services { get; set; } = new List<GymService>();

    public int? GymId { get; set; }

    [ForeignKey("GymId")]
    public virtual Gym? Gym { get; set; }
}