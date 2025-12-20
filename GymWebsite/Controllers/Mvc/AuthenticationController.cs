using GymWebsite.Models.Dto;
using GymWebsite.Models.Users;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace GymWebsite.Controllers.Mvc;

public class AuthenticationController(UserManager<GymUserAccount> userManager, SignInManager<GymUserAccount> signInManager) : Controller {
    public async Task<ActionResult> Register(GymUserAccount account) {
        return View();
    }

    public async Task<ActionResult> Login(UserLogin userLogin)
    {
        var result = await signInManager.PasswordSignInAsync(
            userLogin.Username,
            userLogin.Password,
            isPersistent: userLogin.Persistent,
            lockoutOnFailure: false);

        return result.Succeeded ? View() : Unauthorized();
    }

    public async Task<ActionResult> Logout()
    {
        await signInManager.SignOutAsync();

        return View();
    }
}
