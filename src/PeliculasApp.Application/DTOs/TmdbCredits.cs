using System.Text.Json.Serialization;

namespace PeliculasApp.Application.DTOs;

public class TmdbCastMember
{
    public int Id { get; set; }

    public string Name { get; set; } = "";

    [JsonPropertyName("character")]
    public string? Character { get; set; }

    public string? ProfilePath { get; set; }
}

public class TmdbCredits
{
    public List<TmdbCastMember> Cast { get; set; } = [];
}
