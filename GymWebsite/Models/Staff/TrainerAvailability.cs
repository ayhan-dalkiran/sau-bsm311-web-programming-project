using System.ComponentModel.DataAnnotations;

namespace GymWebsite.Models.Staff;

public class TrainerAvailability {
    public int Id { get; set; }

    // Hangi antrenöre ait olduğunu belirtiyoruz
    public int TrainerId { get; set; }
    public Trainer? Trainer { get; set; }

    // Haftanın hangi günü? (0=Pazar, 1=Pazartesi... 6=Cumartesi)
    [Display(Name = "Haftanın Günü")]
    public DayOfWeek DayOfWeek { get; set; }

    // O gün saat kaçta iş başı yapıyor?
    // TimeSpan sadece saati tutar (Örn: 09:00), tarihi tutmaz.
    [Display(Name = "Başlangıç Saati")]
    [DataType(DataType.Time)]
    public TimeSpan StartTime { get; set; }

    // O gün saat kaçta mesaisi bitiyor?
    [Display(Name = "Bitiş Saati")]
    [DataType(DataType.Time)]
    public TimeSpan EndTime { get; set; }

    // Belki öğle arası vardır veya o gün çalışmıyordur
    [Display(Name = "Müsait mi?")]
    public bool IsAvailable { get; set; } = true;
}