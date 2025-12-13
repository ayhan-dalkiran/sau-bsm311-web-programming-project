using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GymWebsite.Models.Business;

public enum DayOfWeek : int {
    [Display(Name = "Pazartesi")]
    Monday,
    [Display(Name = "Salı")]
    Tuesday,
    [Display(Name = "Çarşamba")]
    Wednesday,
    [Display(Name = "Perşembe")]
    Thursday,
    [Display(Name = "Cuma")]
    Friday,
    [Display(Name = "Cumartesi")]
    Saturday,
    [Display(Name = "Pazar")]
    Sunday,
}

public class GymOpenHourRange {
    [Key]
    public Gym Gym { get; set; }
    [Required]
    public DayOfWeek StartDay { get; set; }
    [Required]
    public TimeOnly StartTime { get; set; }
    [Required]
    public TimeSpan Duration { get; set; }

    [NotMapped]
    public DayOfWeek EndDay {
        get => ;
        set;
    }

}
