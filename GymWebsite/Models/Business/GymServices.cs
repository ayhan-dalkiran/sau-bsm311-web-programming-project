using System.ComponentModel.DataAnnotations;

namespace GymWebsite.Models.Business {
    public class GymService {
        public int Id { get; set; }

        [Display(Name = "Hizmet Adı")]
        [Required(ErrorMessage = "Hizmet adı gereklidir.")]
        public string Name { get; set; } = string.Empty; // Örn: Pilates Seansı, Fitness Üyeliği

        [Display(Name = "Açıklama")]
        public string? Description { get; set; }

        [Display(Name = "Süre (Dakika)")]
        public int DurationMinutes { get; set; } // Örn: 45 dk

        [Display(Name = "Ücret")]
        [DataType(DataType.Currency)]
        public decimal Price { get; set; }

        // Bu hizmet hangi salonda veriliyor? (Tek salon olsa bile ilişki kurmak iyidir)
        public int GymId { get; set; }
        public Gym? Gym { get; set; }
    }
}