namespace PeliculasApp.Application.DTOs;

public class TmdbVideo
{
    public string Key { get; set; } = "";

    public string Name { get; set; } = "";

    public string Site { get; set; } = "";

    public string Type { get; set; } = "";
}

public class TmdbVideoList
{
    public List<TmdbVideo> Results { get; set; } = [];
}
