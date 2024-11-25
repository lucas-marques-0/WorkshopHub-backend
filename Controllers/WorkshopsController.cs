using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

[Route("api/[controller]")]
[ApiController]
public class WorkshopsController : ControllerBase
{
    private static List<Workshop> workshops = new List<Workshop>();

    [HttpPost]
    public ActionResult<Workshop> CreateWorkshop([FromBody] Workshop workshop)
    {
        workshops.Add(workshop);
        return CreatedAtAction(nameof(CreateWorkshop), new { id = workshop.Id }, workshops);
    }
}
