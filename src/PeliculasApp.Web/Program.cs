using PeliculasApp.Application.Interfaces;
using PeliculasApp.Infrastructure.Tmdb;

var builder = WebApplication.CreateBuilder(args);

// TMDB: configuración + cliente HTTP con BaseAddress
builder.Services.Configure<TmdbSettings>(builder.Configuration.GetSection(TmdbSettings.SectionName));
var tmdbSettings = builder.Configuration.GetSection(TmdbSettings.SectionName).Get<TmdbSettings>() ?? new TmdbSettings();
builder.Services.AddHttpClient<ITmdbClient, TmdbClient>(http =>
{
    // El Uri base DEBE terminar en "/": new Uri(base, relative) descarta
    // el último segmento si no termina en barra ("/3" desaparecería).
    http.BaseAddress = new Uri(tmdbSettings.BaseUrl.TrimEnd('/') + "/");
});

// Add services to the container.
builder.Services.AddControllersWithViews();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();


app.Run();
