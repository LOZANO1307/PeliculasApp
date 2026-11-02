using System.Net.Http.Json;
using System.Text.Json;
using Microsoft.Extensions.Options;
using PeliculasApp.Application.DTOs;
using PeliculasApp.Application.Interfaces;

namespace PeliculasApp.Infrastructure.Tmdb;

/// <summary>
/// Implementación de ITmdbClient contra la API v3 de TMDB.
/// Es un "typed client": AddHttpClient le inyecta directamente un HttpClient
/// listo (sin hacer new, sin preocuparse por su ciclo de vida).
/// </summary>
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
}
