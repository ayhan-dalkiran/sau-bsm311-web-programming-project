using System.ComponentModel.DataAnnotations;

namespace GymWebsite.Models.Business {
    public class Gym {
        public int Id { get; set; }

        [Display(Name = "Salon Adı")]
        public string Name { get; set; } = "Benim Spor Salonum";

        [Display(Name = "Adres")]
        public string? Address { get; set; }

        [Display(Name = "Telefon")]
        public string? PhoneNumber { get; set; }

        // Çalışma Saatleri (Basit metin olarak tutabiliriz)
        [Display(Name = "Hafta İçi Çalışma Saatleri")]
        public string OpeningHoursWeekdays { get; set; } = "09:00 - 22:00";

        [Display(Name = "Hafta Sonu Çalışma Saatleri")]
        public string OpeningHoursWeekend { get; set; } = "10:00 - 20:00";

        // Salonun sunduğu hizmetler listesi
        public ICollection<GymService> Services { get; set; } = new List<GymService>();
    }
}