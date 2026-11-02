namespace PeliculasApp.Infrastructure.Tmdb;

/// <summary>
/// Configuración del cliente TMDB. Se enlaza desde la sección "Tmdb" de la
/// configuración; la ApiKey viene de user-secrets (dev) o variables de entorno.
/// </summary>
public class TmdbSettings
{
    public const string SectionName = "Tmdb";

    public string BaseUrl { get; set; } = "https://api.themoviedb.org/3";

    // Nunca va al repo: user-secrets en dev, variable de entorno en prod.
    public string ApiKey { get; set; } = "";

    // Para armar URLs de imágenes: ImageBaseUrl + tamaño + PosterPath.
    public string ImageBaseUrl { get; set; } = "https://image.tmdb.org/t/p/";
}
