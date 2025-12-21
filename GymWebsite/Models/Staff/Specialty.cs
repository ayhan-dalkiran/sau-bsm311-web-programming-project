using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace GymWebsite.Models.Staff;

[Index(nameof(Name), IsUnique = true)]
public class Specialty {
    [Key]
    public int Id { get; set; }

    // Örneğin: "Fitness", "Yoga", "Pilates"
    [Required(AllowEmptyStrings = false)]
    public required string Name { get; set; }
}