using PeliculasApp.Application.DTOs;

namespace PeliculasApp.Application.Interfaces;

// Casos de uso de películas expuestos a la Web.
// De esto depende el controller; de dónde salen los datos (TMDB, BD, ambos)
// es asunto de la capa de abajo.
public interface IMovieService
{
    // Trending semanal.
    Task<TmdbPagedResult> GetTrendingAsync(int page = 1, CancellationToken ct = default);

    // Búsqueda por título.
    Task<TmdbPagedResult> SearchAsync(string query, int page = 1, CancellationToken ct = default);

    Task<MovieDetailBundle> GetDetailAsync(int id, CancellationToken ct = default);
}
