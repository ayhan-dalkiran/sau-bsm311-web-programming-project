using GymWebsite.Models;
using GymWebsite.Models.Business;
using GymWebsite.Models.Users;

using Microsoft.AspNetCore.Cryptography.KeyDerivation;
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
public class UsersController : Controller {
    private GymWebsiteContext Context { get; } = new();

    [HttpPost("login")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public ActionResult<GymUserAuthToken> Login([FromBody] string email, [FromBody] string password, [FromBody] TimeSpan? duration) {
        var user = Context.Users.SingleOrDefault(user => user.Email == email);

        if (user is null)
            return NotFound();

        byte[] hash = KeyDerivation.Pbkdf2(password, user.PasswordSalt, BaseAppUser.Prf, BaseAppUser.Iterations, BaseAppUser.HashSize);

        if (!hash.SequenceEqual(user.PasswordHash))
            return Unauthorized();

        Guid token;
        do
            token = Guid.NewGuid();
        while (Context.GymUserAuthTokens.Any(t => t.AuthToken == token));

        var now = DateTime.UtcNow;

        var authToken = new GymUserAuthToken {
            User = user,
            AuthToken = token,
            CreatedAt = now,
            ExpiresAt = now + (duration ?? TimeSpan.FromHours(12)),
        };

        Context.GymUserAuthTokens.Add(authToken);
        Context.SaveChanges();

        return authToken;
    }

    [HttpPost("logout")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public ActionResult Logout([FromBody] Gym gym) {
        Context.Gyms.Add(gym);
        Context.SaveChanges();

        return CreatedAtAction(nameof(Get), new { id = gym.Id }, null);
    }

    [HttpPatch("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public ActionResult Patch(int id, [FromBody] Gym gym) {
        if (Context.Gyms.Find(id) is null)
            return NotFound();

        Context.Gyms.Update(gym);
        Context.SaveChanges();

        return NoContent();
    }

    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public ActionResult Delete(int id) {
        if (Context.Gyms.Find(id) is not Gym gym)
            return NotFound();

        Context.Gyms.Remove(gym);
        Context.SaveChanges();

        return NoContent();
    }
}
