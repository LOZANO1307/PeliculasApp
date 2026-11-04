namespace PeliculasApp.Web.ViewModels;

// Película lista para mostrar en las vistas: lleva la URL absoluta del póster
// (El DTO de TMDB trae rutas relativas que se arman en el controller).
public class PeliculaVM
{
    public int Id { get; set; }

    public string Title { get; set; } = "";

    public string? PosterUrl { get; set; }

    public string? ReleaseDate { get; set; }

    public double VoteAverage { get; set; }
}

// Para la paginación de las listas.
public class ListaPeliculasVM
{
    public List<PeliculaVM> Peliculas { get; set; } = [];

    public int Page { get; set; }

    public int TotalPages { get; set; }

    public int TotalResults { get; set; }

    // Para que la paginación recuerde la búsqueda ("" en trending).
    public string? Query { get; set; }
}
