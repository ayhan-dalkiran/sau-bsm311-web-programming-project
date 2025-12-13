using Microsoft.AspNetCore.Mvc;

using System.ComponentModel.DataAnnotations;

namespace GymWebsite.Models.Business; 

public class GymService {
    [Key]
    [HiddenInput]
    public int Id { get; set; }

    [Display(Name = "Hizmet Adı")]
    [Required(ErrorMessage = "Hizmet adı gereklidir.")]
    [StringLength(100)]
    public string Name { get; set; } = string.Empty; // Örn: Pilates Seansı, Fitness Üyeliği

    [Display(Name = "Açıklama")]
    [StringLength(4000)]
    public string? Description { get; set; }

    [Display(Name = "Süre (Dakika)")]
    [Range(0, int.MaxValue)]
    public int DurationMinutes { get; set; } // Örn: 45 dk

    [Display(Name = "Ücret")]
    [DataType(DataType.Currency)]
    [Range(0, double.MaxValue)]
    public decimal Price { get; set; }

    public required Gym Gym { get; set; }
}