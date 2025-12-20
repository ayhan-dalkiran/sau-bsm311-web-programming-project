using GymWebsite.Data;
using GymWebsite.Models.Business;
using GymWebsite.Models.Roles;
using GymWebsite.Models.Users;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

internal class Program {
    private static void Main(string[] args) {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.
        string connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
        builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString, sqlOptions => {
        sqlOptions.EnableRetryOnFailure(
            maxRetryCount: 5, // Maksimum 5 kere dene
            maxRetryDelay: TimeSpan.FromSeconds(10), // Denemeler arası 10 saniye bekle
            errorNumbersToAdd: null);
    }));
        builder.Services.AddDatabaseDeveloperPageExceptionFilter();
        builder.Services.AddControllersWithViews();
        builder.Services.AddDefaultIdentity<GymUserAccount>(options => {
                options.Password.RequiredLength = 3;
                options.Password.RequireLowercase = false;
                options.Password.RequireUppercase = false;
                options.Password.RequireDigit = false;
                options.Password.RequireNonAlphanumeric = false;
            })
            .AddRoles<ApplicationUserRole>()
            .AddEntityFrameworkStores<ApplicationDbContext>();

        //builder.Services.AddScoped<I>();

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment()) {
            app.UseMigrationsEndPoint();
        }
        else {
            app.UseExceptionHandler("/Home/Error");
            app.UseHsts();
        }

        app.UseHttpsRedirection();
        app.UseStaticFiles();

        app.UseRouting();

        app.UseAuthorization();
        app.UseAuthentication();

        app.MapControllerRoute(
            name: "default",
            pattern: "{controller=Home}/{action=Index}/{id?}");
        app.MapRazorPages();

        using (var scope = app.Services.CreateScope()) {
            var services = scope.ServiceProvider;
            var userManager = services.GetRequiredService<UserManager<GymUserAccount>>();
            var dbContext = services.GetRequiredService<ApplicationDbContext>();

           //Veritabanı yoksa oluşturur ve bekleyen migrationları uygular.
            dbContext.Database.Migrate();

            if (userManager.FindByEmailAsync("g221210020@sakarya.edu.tr").Result is null) {
                userManager.CreateAsync(new GymUserAccount {
                    UserName = "g221210020",
                    Email = "g221210020@sakarya.edu.tr",
                    PhoneNumber = "901234567890",
                    FirstName = "Admin",
                    LastName = "Adminson",
                    RegistrationDateTime = DateTime.UnixEpoch
                }, "sau").Wait();
            }

            if (userManager.FindByEmailAsync("g231210080@sakarya.edu.tr").Result is null) {
                userManager.CreateAsync(new GymUserAccount {
                    UserName = "g231210080",
                    Email = "g231210080@sakarya.edu.tr",
                    PhoneNumber = "901234567890",
                    FirstName = "Hadmin",
                    LastName = "Hadminson",
                    RegistrationDateTime = DateTime.UnixEpoch
                }, "sau").Wait();
            }

            if (dbContext.Gyms.Find(1) is null) {
                dbContext.Gyms.Add(new Gym {
                    Name = "Power Gym Center",
                    Address = "Atatürk Caddesi, No: 123 Merkez / İstanbul",
                    PhoneNumber = "02125555555",
                    Email = "info@powergym.com",
                });
                dbContext.SaveChanges();
            }
        }

        app.Run();
    }
}