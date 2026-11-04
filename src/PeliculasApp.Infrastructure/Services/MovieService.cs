using PeliculasApp.Application.DTOs;
using PeliculasApp.Application.Interfaces;

namespace PeliculasApp.Infrastructure.Services;

// Aquí irá la mezcla de fuentes (TMDB + nuestra BD) y el cache.
// Por ahora solo delega al cliente de TMDB.
public class MovieService : IMovieService
{
    private readonly ITmdbClient _tmdb;

    public MovieService(ITmdbClient tmdb)
    {
        _tmdb = tmdb;
    }

    public async Task<TmdbPagedResult> GetTrendingAsync(int page = 1, CancellationToken ct = default)
        => await _tmdb.GetTrendingAsync(page, ct);

    public async Task<TmdbPagedResult> SearchAsync(string query, int page = 1, CancellationToken ct = default)
        => await _tmdb.SearchAsync(query, page, ct);
}
