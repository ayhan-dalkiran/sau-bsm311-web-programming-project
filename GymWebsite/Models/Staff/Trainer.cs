using System.ComponentModel.DataAnnotations;

namespace GymWebsite.Models.Staff; 

public class Trainer {
    public int Id { get; set; }

    [Display(Name = "Ad Soyad")]
    public string FullName { get; set; } = string.Empty;

    [Display(Name = "Biyografi")]
    public string? Bio { get; set; }

    // Bir antrenörün birden fazla "Müsaitlik Zamanı" olabilir.
    // (Pazartesi için ayrı, Salı için ayrı satırlar gibi)
    public ICollection<WeekDayTimeSpan> Availabilities { get; set; } = [];

    // Antrenörün yapabildiklerini (Uzmanlıklarını) burada ilişkilendiriyoruz.
    // Bir antrenörün birden fazla uzmanlığı olabilir.
    public ICollection<Specialty> Specialties { get; set; } = [];

    // Antrenörün randevularını görebilmesi için
    public ICollection<Appointment> Appointments { get; set; } = [];
}