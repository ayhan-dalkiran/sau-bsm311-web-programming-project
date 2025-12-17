using GymWebsite.Data;
using GymWebsite.Models;
using GymWebsite.Models.Business;

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
    public ActionResult<List<Gym>> Get(
        [FromQuery, Range(1, 50, ErrorMessage = "itemCount parameter must be between range [1, 50]")] int limit = 10,
        [FromQuery, Range(1, int.MaxValue, ErrorMessage = "")] int offset = 0) {
        var gyms = context.Gyms
            .AsNoTracking()
            .Skip(offset)
            .Take(limit)
            .ToList();

        return gyms;
    }

    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public ActionResult<Gym> Get(int id) {
        var gym = context.Gyms.Find(id);

        return gym is null ? NotFound() : gym;
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public ActionResult<Gym> Post([FromBody] Gym gym) {
        context.Gyms.Add(gym);
        context.SaveChanges();

        return gym;
    }

    [HttpPatch("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public ActionResult<Gym> Patch(int id, [FromBody] Gym gym) {
        if (context.Gyms.Find(id) is null)
            return NotFound();

        gym.Id = id;
        context.Gyms.Update(gym);
        context.SaveChanges();

        return gym;
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
