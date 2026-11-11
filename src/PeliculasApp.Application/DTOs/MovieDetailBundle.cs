using PeliculasApp.Application.DTOs;

namespace PeliculasApp.Application.DTOs;

public class MovieDetailBundle
{
    public TmdbMovieDetail Movie { get; set; } = new();

    public List<TmdbCastMember> Cast { get; set; } = [];

    public string? TrailerKey { get; set; }
}
