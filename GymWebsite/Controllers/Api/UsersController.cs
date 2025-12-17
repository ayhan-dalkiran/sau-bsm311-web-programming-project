using Azure.Core;

using GymWebsite.Data;
using GymWebsite.Models;
using GymWebsite.Models.Business;
using GymWebsite.Models.Dto;
using GymWebsite.Models.Users;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GymWebsite.Controllers.Api;

[ApiController]
[Route("api/[controller]")]
public class UsersController(ApplicationDbContext context, UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager) : Controller {
    [HttpPost("login")]
    [AllowAnonymous]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult> Login(UserLogin userLogin) {
        var result = await signInManager.PasswordSignInAsync(
            userLogin.Username,
            userLogin.Password,
            isPersistent: userLogin.Persistent,
            lockoutOnFailure: false);

        return result.Succeeded ? NoContent() : Unauthorized();
    }

    [HttpPost("logout")]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<ActionResult> Logout() {
        await signInManager.SignOutAsync();

        return NoContent();
    }

    [HttpGet]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult> Get() => Ok(await userManager.Users.ToListAsync());

    [HttpGet("{id}")]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ApplicationUser>> Get(string id) {
        var authUser = await userManager.GetUserAsync(User);

        if (authUser is null)
            return Unauthorized();

        if (!User.IsInRole("Admin") && authUser.Id != id)
            return Unauthorized();

        if (await userManager.FindByIdAsync(id) is not ApplicationUser user)
            return NotFound();

        return Ok(user);
    }

    [HttpPost]
    [AllowAnonymous]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<ActionResult<ApplicationUser>> Post(ApplicationUser user) {
        if (await userManager.FindByNameAsync(user.UserName) is not null)
            return Conflict("Username already exists.");

        var result = await userManager.CreateAsync(user);

        return Ok(user);
    }

    [HttpPatch("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public ActionResult Patch(int id, [FromBody] Gym gym) {
        if (context.Gyms.Find(id) is null)
            return NotFound();

        context.Gyms.Update(gym);
        context.SaveChanges();

        return NoContent();
    }

    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public ActionResult Delete(int id) {
        if (context.Gyms.Find(id) is not Gym gym)
            return NotFound();

        context.Gyms.Remove(gym);
        context.SaveChanges();

        return NoContent();
    }
}
