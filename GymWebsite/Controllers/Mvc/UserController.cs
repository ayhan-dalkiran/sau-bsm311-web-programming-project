using GymWebsite.Data;
using GymWebsite.Models.Business;
using GymWebsite.Models.Dto;
using GymWebsite.Models.Users;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace GymWebsite.Controllers.Mvc; 
public class UserController(ApplicationDbContext context, UserManager<GymUserAccount> userManager, SignInManager<GymUserAccount> signInManager) : Controller {
    public async Task<ActionResult> Register() {
        var currentUser = await userManager.GetUserAsync(User);

        if (currentUser is null)
            return View();

        return RedirectToAction("Index");
    }

    public async Task<ActionResult> Login(UserLogin userLogin) {
        var result = await signInManager.PasswordSignInAsync(
            userLogin.Username,
            userLogin.Password,
            isPersistent: userLogin.Persistent,
            lockoutOnFailure: false);

        if (!result.Succeeded)
            return Unauthorized();

        var user = await userManager.FindByNameAsync(userLogin.Username);

        return RedirectToAction("Index");
    }

    public async Task<ActionResult> Logout() {
        await signInManager.SignOutAsync();

        return RedirectToAction("Index", "Home");
    }

    public async Task<ActionResult> Index() {
        var currentUser = await userManager.GetUserAsync(User);

        if (currentUser is null)
            return RedirectToAction("Register");

        return RedirectToAction("Details", new { id = currentUser.Id });
    }

    [Authorize]
    public async Task<ActionResult> Details(string id) {
        var currentUser = await userManager.GetUserAsync(User);
        
        if (currentUser == null)
            return Unauthorized();

        if (!await userManager.IsInRoleAsync(currentUser, "Admin") && currentUser.Id != id)
            return Unauthorized();

        return View();
    }

    public async Task<ActionResult> Create(GymUserAccount user, string password) {
        var result = await userManager.CreateAsync(user, password);

        if (!result.Succeeded)
            return BadRequest(result.Errors);

        return RedirectToAction("Details", user.Id);
    }

    [Authorize]
    public async Task<ActionResult> Edit(GymUserAccount user) {
        var currentUser = await userManager.GetUserAsync(User);

        if (currentUser == null)
            return Unauthorized();

        if (!await userManager.IsInRoleAsync(currentUser, "Admin") && currentUser.Id != user.Id)
            return Unauthorized();

        var result = await userManager.UpdateAsync(user);

        if (!result.Succeeded)
            return BadRequest(result.Errors);

        return RedirectToAction("Details", user.Id);
    }

    [Authorize]
    public async Task<ActionResult> Delete(string id) {
        var currentUser = await userManager.GetUserAsync(User);

        if (currentUser == null)
            return Unauthorized();

        if (!await userManager.IsInRoleAsync(currentUser, "Admin") && currentUser.Id != id)
            return Unauthorized();

        var targetUser = await userManager.FindByIdAsync(id);

        if (targetUser == null)
            return Unauthorized();

        var result = await userManager.DeleteAsync(targetUser);

        if (!result.Succeeded)
            return BadRequest(result.Errors);

        return RedirectToAction("Login");
    }
}