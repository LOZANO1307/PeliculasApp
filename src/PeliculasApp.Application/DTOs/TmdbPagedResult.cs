namespace PeliculasApp.Application.DTOs;

// Paginación de resultados de TMDB.
public class TmdbPagedResult
{
    public int Page { get; set; }

    public List<TmdbMovieSummary> Results { get; set; } = [];

    public int TotalPages { get; set; }

    public int TotalResults { get; set; }
}
