using System.Text.Json.Serialization;

namespace PeliculasApp.Application.DTOs;

public class TmdbMovieDetail
{
    public int Id { get; set; }

    public string Title { get; set; } = "";

    public string? Overview { get; set; }

    public string? Tagline { get; set; }

    public string? PosterPath { get; set; }

    public string? BackdropPath { get; set; }

    public string? ReleaseDate { get; set; }

    public double VoteAverage { get; set; }

    public int VoteCount { get; set; }

    public int Runtime { get; set; }

    [JsonPropertyName("genres")]
    public List<TmdbGenre> Genres { get; set; } = [];
}

public class TmdbGenre
{
    public int Id { get; set; }

    public string Name { get; set; } = "";
}
