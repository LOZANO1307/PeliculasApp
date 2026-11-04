using PeliculasApp.Application.DTOs;

namespace PeliculasApp.Application.Interfaces;

/// <summary>
/// Casos de uso de películas expuestos a la Web.
/// De esto depende el controller; debajo puede venir de TMDB, de una BD
/// combinada con TMDB o de donde sea — la Web no lo sabe ni le interesa.
/// </summary>
public interface IMovieService
{
    // Trending semanal.
    Task<TmdbPagedResult> GetTrendingAsync(int page = 1, CancellationToken ct = default);

    // Búsqueda por título.
    Task<TmdbPagedResult> SearchAsync(string query, int page = 1, CancellationToken ct = default);
}
