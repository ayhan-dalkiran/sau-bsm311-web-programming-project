using GymWebsite.Data;
using GymWebsite.Models;
using GymWebsite.Models.Business;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace GymWebsite.Controllers.Api;

[ApiController]
[Route("api/[controller]")]
public class GymsController(ApplicationDbContext context) : Controller {
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public IActionResult Get(
        [FromQuery, Range(1, 50, ErrorMessage = "limit parameter must be between range [1, 50]")] int limit = 10,
        [FromQuery, Range(0, int.MaxValue, ErrorMessage = "offset must be positive")] int offset = 0,
        [FromQuery] bool descending = false) {
        var gyms = context.Gyms.AsNoTracking().Skip(offset).Take(limit);

        if (descending)
            gyms.OrderByDescending((Gym gym) => gym.Name);
        else
            gyms.OrderBy((Gym gym) => gym.Name);

        return Ok(gyms.ToList());
    }

    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public IActionResult Get(int id) {
        var gym = context.Gyms.Find(id);

        return gym is null ? NotFound() : Ok(gym);
    }

    [HttpPost]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public IActionResult Post(Gym gym) {
        context.Gyms.Add(gym);
        context.SaveChanges();

        return Ok(gym);
    }

    [HttpPatch("{id}")]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public IActionResult Patch(int id, Gym gym) {
        if (context.Gyms.Find(id) is null)
            return NotFound();

        gym.Id = id;
        context.Gyms.Update(gym);
        context.SaveChanges();

        return Ok(gym);
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public ActionResult Delete(int id) {
        if (context.Gyms.Find(id) is not Gym gym)
            return NotFound();

        context.Gyms.Remove(gym);
        context.SaveChanges();

        return NoContent();
    }
}
