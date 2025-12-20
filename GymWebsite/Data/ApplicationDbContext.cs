using GymWebsite.Models;
using GymWebsite.Models.Business;
using GymWebsite.Models.Staff;
using GymWebsite.Models.Users;

using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace GymWebsite.Data; 
public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : IdentityDbContext(options) {
    public DbSet<Gym> Gyms { get; set; }
    public DbSet<GymService> GymServices { get; set; }
    public DbSet<GymOpenDayTimeSpan> GymOpenDayTimeSpans { get; set; }

    public DbSet<Appointment> Appointments { get; set; }
    public DbSet<Specialty> Specialties { get; set; }
    public DbSet<Trainer> Trainers { get; set; }
    public DbSet<TrainerOpenDayTimeSpan> TrainerOpenDayTimeSpans { get; set; }
}
