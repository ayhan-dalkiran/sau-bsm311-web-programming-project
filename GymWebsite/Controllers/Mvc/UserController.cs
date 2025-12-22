using GymWebsite.Data;
using GymWebsite.Models.Users; // ViewModel'lerin olduğu namespace
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace GymWebsite.Controllers.Mvc;

public class UserController : Controller {

    private readonly UserManager<UserIdentityAccount> _userManager;
    private readonly SignInManager<UserIdentityAccount> _signInManager;
    private readonly ApplicationDbContext _context;

    public UserController(ApplicationDbContext context, UserManager<UserIdentityAccount> userManager, SignInManager<UserIdentityAccount> signInManager) {
        _context = context;
        _userManager = userManager;
        _signInManager = signInManager;
    }

    // ==========================================
    // 1. GİRİŞ VE KAYIT İŞLEMLERİ (AUTH)
    // ==========================================

    [HttpGet]
    public IActionResult Login() {
        if (_signInManager.IsSignedIn(User)) return RedirectToAction("Index", "Home");
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Authenticate(UserLogin userLogin) {
        if (!ModelState.IsValid) {
            return View("Login");
        }

        var result = await _signInManager.PasswordSignInAsync(userLogin.Email, userLogin.Password, userLogin.Persistent, lockoutOnFailure: false);

        if (!result.Succeeded) {
            TempData["ErrorMessage"] = "Geçersiz e-posta veya şifre.";
            return View("Login");
        }

        // --- ROL YÖNLENDİRMESİ ---
        var user = await _userManager.FindByEmailAsync(userLogin.Email);
        if (user != null && await _userManager.IsInRoleAsync(user, "Admin")) {
            return RedirectToAction("Index", "AdminPanel");
        }

        return RedirectToAction("Index", "Home");
    }

    public async Task<IActionResult> Logout() {
        await _signInManager.SignOutAsync();
        return RedirectToAction("Index", "Home");
    }

    [HttpGet]
    public IActionResult Register() {
        if (_signInManager.IsSignedIn(User)) return RedirectToAction("Index", "Home");
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Create(UserDetails userDetails) {
        if (!ModelState.IsValid) return View("Register", userDetails);

        var user = new UserIdentityAccount {
            UserName = userDetails.Email,
            Email = userDetails.Email,
            FirstName = userDetails.FirstName,
            LastName = userDetails.LastName,
            PhoneNumber = userDetails.PhoneNumber,
            RegistrationDateTime = DateTime.Now
        };

        var result = await _userManager.CreateAsync(user, userDetails.Password);

        if (result.Succeeded) {
            // Kayıt olur olmaz otomatik 'Member' rolü verelim
            await _userManager.AddToRoleAsync(user, "Member");

            // Otomatik giriş yaptırabiliriz veya login'e atabiliriz
            await _signInManager.SignInAsync(user, isPersistent: false);
            return RedirectToAction("Index", "Home");
        }

        foreach (var error in result.Errors) {
            ModelState.AddModelError("", error.Description);
        }

        return View("Register", userDetails);
    }


    // ==========================================
    // 2. KULLANICI PROFİL İŞLEMLERİ
    // ==========================================

    // Hesabım (Dashboard) Sayfası
    [Authorize]
    public async Task<IActionResult> Profile() {
        var user = await _userManager.GetUserAsync(User);
        if (user == null) return RedirectToAction("Login");

        return View(user); // Views/User/Profile.cshtml
    }

    // Profil Düzenleme (GET)
    [Authorize]
    public async Task<IActionResult> EditProfile() {
        var user = await _userManager.GetUserAsync(User);
        if (user == null) return RedirectToAction("Login");

        var model = new UserEditViewModel {
            FirstName = user.FirstName,
            LastName = user.LastName,
            PhoneNumber = user.PhoneNumber
        };

        return View(model); // Views/User/EditProfile.cshtml
    }

    // Profil Düzenleme (POST)
    [HttpPost]
    [Authorize]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> EditProfile(UserEditViewModel model) {
        if (!ModelState.IsValid) return View(model);

        var user = await _userManager.GetUserAsync(User);
        if (user == null) return RedirectToAction("Login");

        // Bilgileri güncelle
        user.FirstName = model.FirstName;
        user.LastName = model.LastName;
        user.PhoneNumber = model.PhoneNumber;

        var result = await _userManager.UpdateAsync(user);

        if (result.Succeeded) {
            TempData["SuccessMessage"] = "Profil bilgileriniz güncellendi.";
            return RedirectToAction(nameof(Profile));
        }

        foreach (var error in result.Errors) ModelState.AddModelError("", error.Description);
        return View(model);
    }

    // Şifre Değiştirme (GET)
    [Authorize]
    public IActionResult ChangePassword() {
        return View(); // Views/User/ChangePassword.cshtml
    }

    // Şifre Değiştirme (POST)
    [HttpPost]
    [Authorize]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> ChangePassword(ChangePasswordViewModel model) {
        if (!ModelState.IsValid) return View(model);

        var user = await _userManager.GetUserAsync(User);
        if (user == null) return RedirectToAction("Login");

        // Eski şifreyi kontrol edip değiştirme işlemi
        var result = await _userManager.ChangePasswordAsync(user, model.OldPassword, model.NewPassword);

        if (result.Succeeded) {
            await _signInManager.RefreshSignInAsync(user); // Oturumu yenile
            TempData["SuccessMessage"] = "Şifreniz başarıyla değiştirildi.";
            return RedirectToAction(nameof(Profile));
        }

        foreach (var error in result.Errors) {
            ModelState.AddModelError("", error.Description);
        }

        return View(model);
    }
}