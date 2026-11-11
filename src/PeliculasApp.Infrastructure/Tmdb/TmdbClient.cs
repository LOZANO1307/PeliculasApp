using System.Net.Http.Json;
using System.Text.Json;
using Microsoft.Extensions.Options;
using PeliculasApp.Application.DTOs;
using PeliculasApp.Application.Interfaces;

namespace PeliculasApp.Infrastructure.Tmdb;

// Cliente HTTP para TMDB. Es un "typed client": AddHttpClient le inyecta
// directamente el HttpClient (sin new, sin manejar su ciclo de vida).
public class TmdbClient : ITmdbClient
{
    private readonly HttpClient _http;
    private readonly TmdbSettings _settings;

    // TMDB responde con nombres JSON en snake_case ("poster_path").
    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower,
        PropertyNameCaseInsensitive = true,
    };

    public TmdbClient(HttpClient http, IOptions<TmdbSettings> options)
    {
        _http = http;
        _settings = options.Value;
    }

    public async Task<TmdbPagedResult> GetTrendingAsync(int page = 1, CancellationToken ct = default)
    {
        var url = $"trending/movie/week?page={page}&api_key={_settings.ApiKey}";
        var response = await _http.GetAsync(url, ct);
        // TODO: mostrar algo amable en la UI si TMDB falla, en vez de tirar 500
        response.EnsureSuccessStatusCode();
        return (await response.Content.ReadFromJsonAsync<TmdbPagedResult>(JsonOptions, ct))!;
    }

    public async Task<TmdbPagedResult> SearchAsync(string query, int page = 1, CancellationToken ct = default)
    {
        var url = $"search/movie?query={Uri.EscapeDataString(query)}&page={page}&api_key={_settings.ApiKey}";
        var response = await _http.GetAsync(url, ct);
        response.EnsureSuccessStatusCode();
        return (await response.Content.ReadFromJsonAsync<TmdbPagedResult>(JsonOptions, ct))!;
    }

    public async Task<TmdbMovieDetail> GetDetailAsync(int id, CancellationToken ct = default)
        => await GetAsync<TmdbMovieDetail>($"movie/{id}", ct);

    public async Task<TmdbCredits> GetCreditsAsync(int id, CancellationToken ct = default)
        => await GetAsync<TmdbCredits>($"movie/{id}/credits", ct);

    public async Task<TmdbVideoList> GetVideosAsync(int id, CancellationToken ct = default)
        => await GetAsync<TmdbVideoList>($"movie/{id}/videos", ct);

    private async Task<T> GetAsync<T>(string resource, CancellationToken ct)
    {
        var url = $"{resource}?api_key={_settings.ApiKey}";
        var response = await _http.GetAsync(url, ct);
        response.EnsureSuccessStatusCode();
        return (await response.Content.ReadFromJsonAsync<T>(JsonOptions, ct))!;
    }
}
