using GymWebsite.Models.Business;
using GymWebsite.Models.Members;
using GymWebsite.Models.Staff;

using Microsoft.EntityFrameworkCore;

namespace GymWebsite.Models;

public class GymWebsiteContext : DbContext {
    public DbSet<Gym> Gyms { get; set; }
    public DbSet<GymService> GymServices { get; set; }
    public DbSet<GymOpenDayTimeSpan> GymOpenDayTimeSpans { get; set; }

    public DbSet<GymMember> GymMembers { get; set; }
    public DbSet<Appointment> Appointments { get; set; }
    public DbSet<Specialty> Specialties { get; set; }
    public DbSet<Trainer> Trainers { get; set; }
    public DbSet<TrainerOpenDayTimeSpan> TrainerOpenDayTimeSpans { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=GymWebsiteDB;Trusted_Connection=True;");
}
