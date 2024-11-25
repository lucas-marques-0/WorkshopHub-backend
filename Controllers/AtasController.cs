using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;

[Route("api/[controller]")]
[ApiController]
public class AtasController : ControllerBase
{
    public static List<Ata> atas = new List<Ata>();

    [HttpPost]
    public ActionResult<Ata> CreateAta([FromBody] Ata ata)
    {
        atas.Add(ata);
        return CreatedAtAction(nameof(CreateAta), new { id = ata.Id }, atas);
    }

    [HttpPut("{ataId}/colaboradores/{colaboradorId}")]
    public IActionResult AddColaboradorToAta(int ataId, int colaboradorId)
    {
        var ata = atas.FirstOrDefault(a => a.Id == ataId);
        if (ata == null)
        {
            return NotFound();
        }

        var colaborador = ColaboradoresController.Colaboradores.FirstOrDefault(c => c.Id == colaboradorId);
        if (colaborador == null)
        {
            return NotFound("Colaborador não encontrado.");
        }

        ata.Colaboradores.Add(colaborador);
        return CreatedAtAction(nameof(CreateAta), new { id = ata.Id }, atas);
    }

    [HttpDelete("{ataId}/colaboradores/{colaboradorId}")]
    public IActionResult RemoveColaboradorFromAta(int ataId, int colaboradorId)
    {
        var ata = atas.FirstOrDefault(a => a.Id == ataId);
        if (ata == null)
        {
            return NotFound();
        }
        var colaborador = ColaboradoresController.Colaboradores.FirstOrDefault(c => c.Id == colaboradorId);
        if (colaborador == null)
        {
            return NotFound("Colaborador não encontrado.");
        }
        ata.Colaboradores.Remove(colaborador);
        return CreatedAtAction(nameof(CreateAta), new { id = ata.Id }, atas);
    }

    [HttpGet]
    public ActionResult<List<Ata>> GetAtas([FromQuery] string workshopNome = null, [FromQuery] string data = null)
    {
        var result = atas.AsEnumerable();
        if (!string.IsNullOrEmpty(workshopNome))
        {
            result = result.Where(a => a.Workshop.Nome.Contains(workshopNome));
        }
        if (!string.IsNullOrEmpty(data) && DateTime.TryParse(data, out var parsedDate))
        {
            result = result.Where(a => a.Workshop.DataRealizacao.Date == parsedDate.Date);
        }
        return Ok(result);
    }

    [HttpGet("get-by-name")]
    public ActionResult<List<object>> GetAtasByWorkshopNome([FromQuery] string workshopNome)
    {
        var filteredAtas = atas
            .Where(a => a.Workshop.Nome.Contains(workshopNome, StringComparison.OrdinalIgnoreCase))
            .ToList();

        if (!filteredAtas.Any())
        {
            return NotFound("Nenhuma ata registrada para o workshop com esse nome.");
        }

        var result = filteredAtas
            .SelectMany(a => a.Colaboradores)
            .OrderBy(c => c.Nome)
            .Select(c => new { c.Id, c.Nome }) 
            .ToList();

        return Ok(result);
    }

    [HttpGet("get-by-date")]
    public ActionResult GetAtasByData([FromQuery] string workshopDate)
    {
        if (!DateTime.TryParse(workshopDate, out var parsedDate))
        {
            return BadRequest("A data fornecida é inválida.");
        }

        var filteredAtas = atas
            .Where(a => a.Workshop.DataRealizacao.Date == parsedDate.Date)
            .ToList();

        if (!filteredAtas.Any())
        {
            return NotFound("Nenhum workshop encontrado para a data informada.");
        }

        var result = filteredAtas
            .SelectMany(a => a.Colaboradores)
            .OrderBy(c => c.Nome)
            .Select(c => new { c.Id, c.Nome })
            .ToList();

        return Ok(result);
    }
}
