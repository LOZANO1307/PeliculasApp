using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using PeliculasApp.Application.DTOs;
using PeliculasApp.Application.Interfaces;
using PeliculasApp.Infrastructure.Tmdb;
using PeliculasApp.Web.ViewModels;

namespace PeliculasApp.Web.Controllers;

public class MoviesController : Controller
{
    private readonly IMovieService _movies;
    private readonly TmdbSettings _settings;

    public MoviesController(IMovieService movies, IOptions<TmdbSettings> options)
    {
        _movies = movies;
        _settings = options.Value;
    }

    // GET /Movies  (trending de la semana)
    public async Task<IActionResult> Index(int page = 1, CancellationToken ct = default)
    {
        page = Math.Max(1, page);
        var result = await _movies.GetTrendingAsync(page, ct);
        var vm = ToVM(result);
        return View(vm);
    }

    // GET /Movies/Search?q=...
    [HttpGet("Movies/Search")]
    public async Task<IActionResult> Search(string? q, int page = 1, CancellationToken ct = default)
    {
        page = Math.Max(1, page);
        var result = string.IsNullOrWhiteSpace(q)
            ? await _movies.GetTrendingAsync(page, ct)
            : await _movies.SearchAsync(q.Trim(), page, ct);
        var vm = ToVM(result);
        vm.Query = string.IsNullOrWhiteSpace(q) ? null : q.Trim();
        return View("Index", vm);
    }

    [HttpGet("Movies/Detail/{id:int}")]
    public async Task<IActionResult> Detail(int id, CancellationToken ct = default)
    {
        var bundle = await _movies.GetDetailAsync(id, ct);
        var m = bundle.Movie;

        var vm = new DetallePeliculaVM
        {
            Id = m.Id,
            Title = m.Title,
            Tagline = m.Tagline,
            Overview = m.Overview,
            PosterUrl = m.PosterPath is null ? null : $"{_settings.ImageBaseUrl}w342{m.PosterPath}",
            BackdropUrl = m.BackdropPath is null ? null : $"{_settings.ImageBaseUrl}w1280{m.BackdropPath}",
            ReleaseDate = m.ReleaseDate,
            VoteAverage = m.VoteAverage,
            VoteCount = m.VoteCount,
            Runtime = m.Runtime,
            Genres = m.Genres,
            TrailerKey = bundle.TrailerKey,
            Cast = bundle.Cast.Take(12).Select(c => new ActorVM
            {
                Name = c.Name,
                Character = c.Character,
                ProfileUrl = c.ProfilePath is null ? null : $"{_settings.ImageBaseUrl}w185{c.ProfilePath}",
            }).ToList(),
        };
        return View(vm);
    }

    private ListaPeliculasVM ToVM(TmdbPagedResult result)
    {
        var tt = _settings.ImageBaseUrl;
        return new ListaPeliculasVM
        {
            Peliculas = [.. result.Results.Select(m => new PeliculaVM
            {
                Id = m.Id,
                Title = m.Title,
                // w342 = tamaño de póster de TMDB ("w185", "w342", "w500"...)
                PosterUrl = m.PosterPath is null ? null : $"{tt}w342{m.PosterPath}",
                ReleaseDate = m.ReleaseDate,
                VoteAverage = m.VoteAverage,
            })],
            Page = result.Page,
            TotalPages = Math.Min(result.TotalPages, 100), // TMDB corta en 500; la UI basta con 100
            TotalResults = result.TotalResults,
        };
    }
}
