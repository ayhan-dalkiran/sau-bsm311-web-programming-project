using GymWebsite.Data;
using GymWebsite.Models.Business;
using GymWebsite.Models.Users;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

internal class Program {
    // Main metodunu 'async Task' yaptık ki içeride 'await' kullanabilelim (Daha sağlıklı)
    private static async Task Main(string[] args) {
        var builder = WebApplication.CreateBuilder(args);

        // 1. Veritabanı Bağlantısı
        string connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
            ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

        builder.Services.AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlServer(connectionString, sqlOptions => {
                sqlOptions.EnableRetryOnFailure(
                    maxRetryCount: 5,
                    maxRetryDelay: TimeSpan.FromSeconds(10),
                    errorNumbersToAdd: null);
            }));

        builder.Services.AddDatabaseDeveloperPageExceptionFilter();
        builder.Services.AddControllersWithViews();

        // 2. Identity Ayarları (Şifre Kuralları ve Rol Yönetimi)
        builder.Services.AddDefaultIdentity<UserIdentityAccount>(options => {
            // Şifre kurallarını esnetiyoruz (Ödev için 'sau' şifresine izin vermek adına)
            options.Password.RequiredLength = 3;
            options.Password.RequireLowercase = false;
            options.Password.RequireUppercase = false;
            options.Password.RequireDigit = false;
            options.Password.RequireNonAlphanumeric = false;
            options.SignIn.RequireConfirmedAccount = false;
        })
            .AddRoles<IdentityRole>() // <--- KRİTİK: Rol yöneticisini aktif etmek için bu şart!
            .AddEntityFrameworkStores<ApplicationDbContext>();

        var app = builder.Build();

        // 3. Middleware Ayarları
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

        app.UseAuthentication(); // Kimlik Doğrulama
        app.UseAuthorization();  // Yetkilendirme

        app.MapControllerRoute(
            name: "default",
            pattern: "{controller=Home}/{action=Index}/{id?}");
        app.MapRazorPages();

        // 4. BAŞLANGIÇ VERİLERİ (SEEDING)
        // Uygulama her başladığında burası çalışır ve eksikleri tamamlar.
        using (var scope = app.Services.CreateScope()) {
            var services = scope.ServiceProvider;
            try {
                var dbContext = services.GetRequiredService<ApplicationDbContext>();
                var userManager = services.GetRequiredService<UserManager<UserIdentityAccount>>();
                var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();

                // A. Veritabanını Güncelle (Migration'ları uygula)
                await dbContext.Database.MigrateAsync();

                // B. Rolleri Oluştur (Eğer yoksa)
                if (!await roleManager.RoleExistsAsync("Admin"))
                    await roleManager.CreateAsync(new IdentityRole("Admin"));

                if (!await roleManager.RoleExistsAsync("Member"))
                    await roleManager.CreateAsync(new IdentityRole("Member"));

                // C. Admin Kullanıcısı 1 Oluşturma
                string admin1Email = "g221210020@sakarya.edu.tr";
                if (await userManager.FindByEmailAsync(admin1Email) == null) {
                    var user = new UserIdentityAccount {
                        UserName = admin1Email,
                        Email = admin1Email,
                        PhoneNumber = "905551112233",
                        FirstName = "Admin",
                        LastName = "User 1",
                        RegistrationDateTime = DateTime.Now,
                        EmailConfirmed = true
                    };
                    // Kullanıcıyı oluştur
                    var result = await userManager.CreateAsync(user, "sau");
                    // Rol Ata
                    if (result.Succeeded) await userManager.AddToRoleAsync(user, "Admin");
                }

                // D. Admin Kullanıcısı 2 Oluşturma
                string admin2Email = "g231210080@sakarya.edu.tr";
                if (await userManager.FindByEmailAsync(admin2Email) == null) {
                    var user = new UserIdentityAccount {
                        UserName = admin2Email,
                        Email = admin2Email,
                        PhoneNumber = "905554445566",
                        FirstName = "Hadmin",
                        LastName = "User 2",
                        RegistrationDateTime = DateTime.Now,
                        EmailConfirmed = true
                    };
                    var result = await userManager.CreateAsync(user, "sau");
                    if (result.Succeeded) await userManager.AddToRoleAsync(user, "Admin");
                }

                // E. Varsayılan Spor Salonu Oluşturma
                if (!dbContext.Gyms.Any()) {
                    dbContext.Gyms.Add(new Gym {
                        Name = "Power Gym Center",
                        Address = "Atatürk Caddesi, No: 123\nMerkez / İstanbul",
                        PhoneNumber = "0212 555 55 55",
                        Email = "info@powergym.com",
                    });
                    await dbContext.SaveChangesAsync();
                }
            }
            catch (Exception ex) {
                // Hata olursa konsola yaz (Loglama)
                var logger = services.GetRequiredService<ILogger<Program>>();
                logger.LogError(ex, "Veritabanı oluşturulurken hata meydana geldi.");
            }
        }

        await app.RunAsync();
    }
}