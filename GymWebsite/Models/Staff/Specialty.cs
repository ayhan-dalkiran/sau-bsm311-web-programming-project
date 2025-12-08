namespace GymWebsite.Models.Staff {
    public class Specialty {
        public int Id { get; set; }

        // Örneğin: "Fitness", "Yoga", "Pilates"
        public string Name { get; set; } = string.Empty;

        // Bu uzmanlığa sahip antrenörleri listelemek için (İsteğe bağlı, ilişki için)
        public ICollection<Trainer>? Trainers { get; set; }
    }
}