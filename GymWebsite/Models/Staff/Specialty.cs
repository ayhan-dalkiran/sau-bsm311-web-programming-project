using System.ComponentModel.DataAnnotations;

namespace GymWebsite.Models.Staff; 
public class Specialty {
    [Key]
    public int Id { get; set; }

    // Örneğin: "Fitness", "Yoga", "Pilates"
    public string Name { get; set; } = string.Empty;
}