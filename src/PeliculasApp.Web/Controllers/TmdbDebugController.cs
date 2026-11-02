using Microsoft.AspNetCore.Mvc;
using PeliculasApp.Application.Interfaces;

namespace PeliculasApp.Web.Controllers;

/// <summary>
/// Controlador temporal para verificar la conexión con TMDB mientras el home
/// no existe todavía. Se elimina cuando /Movies esté funcionando.
/// </summary>
[ApiController]
[Route("api/debug/tmdb")]
public class TmdbDebugController : ControllerBase
{
    private readonly ITmdbClient _tmdb;

    public TmdbDebugController(ITmdbClient tmdb)
    {
        _tmdb = tmdb;
    }

    // GET api/debug/tmdb/trending
    [HttpGet("trending")]
    public async Task<IActionResult> Trending(CancellationToken ct)
    {
        var result = await _tmdb.GetTrendingAsync(ct: ct);
        return Ok(new { total_pelis = result.TotalResults, pagina = result.Page, peliculas = result.Results });
    }

    // TODO: probar SearchAsync desde el front cuando exista la barra de búsqueda
}
