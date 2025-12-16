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
public class GymsController : Controller {
    private GymWebsiteContext Context { get; } = new();

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public ActionResult<List<Gym>> Get(
        [FromQuery, Range(1, 50, ErrorMessage = "itemCount parameter must be between range [1, 50]")] int limit = 10,
        [FromQuery, Range(1, int.MaxValue, ErrorMessage = "")] int offset = 0) {
        var gyms = Context.Gyms
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
        var gym = Context.Gyms.Find(id);

        return gym is null ? NotFound() : gym;
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public ActionResult<Gym> Post([FromBody] Gym gym) {
        Context.Gyms.Add(gym);
        Context.SaveChanges();

        return gym;
    }

    [HttpPatch("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public ActionResult<Gym> Patch(int id, [FromBody] Gym gym) {
        if (Context.Gyms.Find(id) is null)
            return NotFound();

        gym.Id = id;
        Context.Gyms.Update(gym);
        Context.SaveChanges();

        return gym;
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
