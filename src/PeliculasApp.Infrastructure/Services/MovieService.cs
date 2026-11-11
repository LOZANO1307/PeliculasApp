using Microsoft.Extensions.Caching.Memory;
using PeliculasApp.Application.DTOs;
using PeliculasApp.Application.Interfaces;

namespace PeliculasApp.Infrastructure.Services;

public class MovieService : IMovieService
{
    private readonly ITmdbClient _tmdb;
    private readonly IMemoryCache _cache;

    public MovieService(ITmdbClient tmdb, IMemoryCache cache)
    {
        _tmdb = tmdb;
        _cache = cache;
    }

    public async Task<TmdbPagedResult> GetTrendingAsync(int page = 1, CancellationToken ct = default)
    {
        return await _cache.GetOrCreateAsync($"trending:p{page}", async entry =>
        {
            entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(1);
            return await _tmdb.GetTrendingAsync(page, ct);
        }) ?? new TmdbPagedResult();
    }

    public async Task<TmdbPagedResult> SearchAsync(string query, int page = 1, CancellationToken ct = default)
    {
        var key = $"search:{query.ToLowerInvariant()}:p{page}";
        return await _cache.GetOrCreateAsync(key, async entry =>
        {
            entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(15);
            return await _tmdb.SearchAsync(query, page, ct);
        }) ?? new TmdbPagedResult();
    }

    public async Task<MovieDetailBundle> GetDetailAsync(int id, CancellationToken ct = default)
    {
        return await _cache.GetOrCreateAsync($"detail:{id}", async entry =>
        {
            entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(30);
            return await BuildDetailAsync(id, ct);
        }) ?? new MovieDetailBundle();
    }

    private async Task<MovieDetailBundle> BuildDetailAsync(int id, CancellationToken ct)
    {
        var movieTask = _tmdb.GetDetailAsync(id, ct);
        var creditsTask = _tmdb.GetCreditsAsync(id, ct);
        var videosTask = _tmdb.GetVideosAsync(id, ct);
        await Task.WhenAll(movieTask, creditsTask, videosTask);

        return new MovieDetailBundle
        {
            Movie = movieTask.Result,
            Cast = creditsTask.Result.Cast,
            TrailerKey = videosTask.Result.Results
                .FirstOrDefault(v => v.Site == "YouTube" && v.Type == "Trailer")?.Key,
        };
    }
}
