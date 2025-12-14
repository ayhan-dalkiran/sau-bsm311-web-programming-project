using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

using System.ComponentModel.DataAnnotations;

namespace GymWebsite.Models.Business; 

public class GymService {
    [Key]
    [HiddenInput]
    public int Id { get; set; }

    public required Gym Gym { get; set; }

    [Unicode]
    [Required(ErrorMessage = "Hizmet adı boş olamaz.")]
    [MaxLength(100, ErrorMessage = "Hizmet adı 100 karakterden fazla olamaz.")]
    [Display(Name = "Hizmet Adı")]
    public required string Name { get; set; } // Örn: Pilates Seansı, Fitness Üyeliği

    [Unicode]
    [MaxLength(4000, ErrorMessage = "Hizmet açıklaması 4000 karakterden fazla olamaz.")]
    [Display(Name = "Açıklama")]
    public string? Description { get; set; }

    [Required(ErrorMessage = "Hizmet süresi boş olamaz.")]
    [Range(0, int.MaxValue, ErrorMessage = "Hizmet süresi negatif bir sayı olamaz.")]
    [Display(Name = "Süre (Dakika)")]
    public int DurationInMinutes { get; set; } // Örn: 45 dk

    [Required(ErrorMessage = "Hizmet ücreti boş olamaz.")]
    [Precision(17, 2)]
    [Range(0, 15e9, ErrorMessage = "Hizmet ücreti pozitif ve 15 haneden küçük olmalıdır.")]
    [Display(Name = "Ücret")]
    public decimal Price { get; set; }
}