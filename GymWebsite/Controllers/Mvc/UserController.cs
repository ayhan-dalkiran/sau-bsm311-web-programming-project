using GymWebsite.Data;
using GymWebsite.Models.Business;
using GymWebsite.Models.Dto;
using GymWebsite.Models.Users;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace GymWebsite.Controllers.Mvc; 
public class UserController(ApplicationDbContext context, UserManager<GymUserAccount> userManager, SignInManager<GymUserAccount> signInManager) : Controller {
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