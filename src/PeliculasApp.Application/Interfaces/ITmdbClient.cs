using PeliculasApp.Application.DTOs;

namespace PeliculasApp.Application.Interfaces;

// La Web solo conoce esta interfaz; la implementación (HTTP real) va en Infrastructure.
public interface ITmdbClient
{
    // Trending semanal de TMDB. page para paginar.
    Task<TmdbPagedResult> GetTrendingAsync(int page = 1, CancellationToken ct = default);

    // Búsqueda por título.
    Task<TmdbPagedResult> SearchAsync(string query, int page = 1, CancellationToken ct = default);

    Task<TmdbMovieDetail> GetDetailAsync(int id, CancellationToken ct = default);

    Task<TmdbCredits> GetCreditsAsync(int id, CancellationToken ct = default);

    Task<TmdbVideoList> GetVideosAsync(int id, CancellationToken ct = default);
}
