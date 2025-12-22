using GymWebsite.Data;
using GymWebsite.Models.Business;
using GymWebsite.Models.Users;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace GymWebsite.Controllers.Mvc; 
public class UserController(ApplicationDbContext context, UserManager<UserIdentityAccount> userManager, SignInManager<UserIdentityAccount> signInManager) : Controller {
    public async Task<ActionResult> Login() {
        var currentUser = await userManager.GetUserAsync(User);

        if (currentUser is null)
            return View();

        return RedirectToAction("Index");
    }

    [HttpPost] // Bu attribute eksikti, eklemek iyi olur
    public async Task<ActionResult> Authenticate(UserLogin userLogin) {
        if (!ModelState.IsValid) {
            TempData["ErrorMessage"] = "Geçersiz kullanıcı adı veya şifre";
            return RedirectToAction("Login");
        }

        // Giriş yapma işlemi
        var result = await signInManager.PasswordSignInAsync(userLogin.Email, userLogin.Password, userLogin.Persistent, lockoutOnFailure: false);

        if (!result.Succeeded) {
            TempData["ErrorMessage"] = "Geçersiz kullanıcı adı veya şifre";
            return RedirectToAction("Login");
        }

        // --- ROL KONTROLÜ BAŞLANGIÇ ---

        // 1. Giriş yapan kullanıcıyı buluyoruz
        var user = await userManager.FindByEmailAsync(userLogin.Email);

        // 2. Kullanıcı bulunduysa ve "Admin" rolündeyse Admin Paneline yönlendir
        if (user != null && await userManager.IsInRoleAsync(user, "Admin")) {
            // AdminPanelController'ın 'Index' veya 'Gym' metoduna yönlendir.
            // Eğer Dashboard yaptıysak 'Index', direkt salon bilgileriyse 'Gym' yazabilirsin.
            return RedirectToAction("Index", "AdminPanel");
        }

        // --- ROL KONTROLÜ BİTİŞ ---

        // Admin değilse normal Ana Sayfaya yönlendir
        return RedirectToAction("Index", "Home");
    }

    public async Task<ActionResult> Logout() {
        await signInManager.SignOutAsync();

        return RedirectToAction("Index", "Home");
    }

    public async Task<ActionResult> Index() {
        if (!signInManager.IsSignedIn(User))
            return RedirectToAction("Register");

        return RedirectToAction("Details");
    }

    //Read (View)
    //Update (View)
    [Authorize]
    public async Task<ActionResult> Details() {
        var currentUser = await userManager.GetUserAsync(User);

        return View(currentUser!);
    }

    //Create (View)
    public async Task<ActionResult> Register() {
        if (!signInManager.IsSignedIn(User))
            return View();

        return RedirectToAction("Index");
    }

    //Create
    public async Task<ActionResult> Create(UserDetails userDetails) {
        if (!ModelState.IsValid) {
            TempData["ErrorMessage"] = "Hata! Kullanıcı oluşturulamadı";
            return RedirectToAction("Register");
        }

        var user = new UserIdentityAccount {
            UserName = userDetails.Email,
            Email = userDetails.Email,
            FirstName = userDetails.FirstName,
            LastName = userDetails.LastName,
            PhoneNumber = userDetails.PhoneNumber,
            RegistrationDateTime = DateTime.UtcNow
        };
        var result = await userManager.CreateAsync(user, userDetails.Password);

        if (!result.Succeeded) {
            TempData["ErrorMessage"] = "Hata! Kullanıcı oluşturulamadı";
            return RedirectToAction("Register");
        }

        return RedirectToAction("Details");
    }

    //Update
    [Authorize]
    public async Task<ActionResult> Edit(UserDetails userDetails) {
        if (!ModelState.IsValid) {
            TempData["ErrorMessage"] = "Hata! Kullanıcı ayarları değiştirilemedi";
            return RedirectToAction("Details");
        }

        var user = (await userManager.GetUserAsync(User))!;
        user.Email = userDetails.Email;


        var result = await userManager.UpdateAsync(user);

        if (!result.Succeeded) {
            TempData["ErrorMessage"] = "Hata! Kullanıcı ayarları değiştirilemedi";
            return RedirectToAction("Details", user.Id);
        }

        return RedirectToAction("Details", user.Id);
    }

    [Authorize]
    public async Task<ActionResult> Delete() {
        var currentUser = await userManager.GetUserAsync(User);

        if (currentUser == null)
            return Unauthorized();

        var result = await userManager.DeleteAsync(currentUser!);

        if (!result.Succeeded) {
            TempData["ErrorMessage"] = "Hata! Kullanıcı silinemedi";
            return RedirectToAction("Details", currentUser.Id);
        }

        return RedirectToAction("Index", "Home");
    }
}