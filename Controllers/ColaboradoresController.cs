using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;

[Route("api/[controller]")]
[ApiController]
public class ColaboradoresController : ControllerBase
{
    public static List<Colaborador> Colaboradores = new List<Colaborador>();

    [HttpPost]
    public ActionResult<Colaborador> CreateColaborador([FromBody] Colaborador colaborador)
    {
        Colaboradores.Add(colaborador);
        return CreatedAtAction(nameof(CreateColaborador), new { id = colaborador.Id }, Colaboradores);
    }

    [HttpGet]
    public ActionResult<List<object>> GetColaboradores()
    {
        var atas = AtasController.atas;

        var result = Colaboradores
            .OrderBy(c => c.Nome) 
            .Select(c => new
            {
                Colaborador = c,
                Workshops = atas
                    .Where(a => a.Colaboradores.Any(col => col.Id == c.Id)) 
                    .Select(a => a.Workshop) 
                    .ToList()
            })
            .ToList();

        return Ok(result);
    }
}
