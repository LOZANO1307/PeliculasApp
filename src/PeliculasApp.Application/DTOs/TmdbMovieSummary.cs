namespace PeliculasApp.Application.DTOs;

// Estructura de una película de TMDB (trending, búsqueda, etc).
// El JSON llega en snake_case; TmdbClient lo mapea con SnakeCaseLower.
public class TmdbMovieSummary
{
    public int Id { get; set; }

    public string Title { get; set; } = "";

    public string? Overview { get; set; }

    // Ruta relativa del póster ("/abc123.jpg"); la URL completa se arma con
    // TmdbSettings.ImageBaseUrl + tamaño deseado.
    public string? PosterPath { get; set; }

    public string? BackdropPath { get; set; }

    public string? ReleaseDate { get; set; }

    public double VoteAverage { get; set; }
}
