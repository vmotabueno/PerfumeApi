using Microsoft.EntityFrameworkCore;
using PerfumeApi.Data;
using PerfumeApi.DTOs;
using PerfumeApi.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

var app = builder.Build();

app.MapGet("/", () => "API de Perfumes rodando.");

app.MapPost("/perfumes", async (CreatePerfumeRequest request, AppDbContext db) =>
{
    if (string.IsNullOrWhiteSpace(request.Nome))
        return Results.BadRequest("O campo Nome é obrigatório.");

    if (string.IsNullOrWhiteSpace(request.Marca))
        return Results.BadRequest("O campo Marca é obrigatório.");

    if (string.IsNullOrWhiteSpace(request.ImageUrl))
        return Results.BadRequest("O campo ImageUrl é obrigatório.");

    var perfume = new Perfume
    {
        Nome = request.Nome.Trim(),
        Marca = request.Marca.Trim(),
        ImageUrl = request.ImageUrl.Trim()
    };

    db.Perfumes.Add(perfume);
    await db.SaveChangesAsync();

    return Results.Created($"/perfumes/{perfume.Id}", perfume);
});

app.MapGet("/perfumes", async (AppDbContext db) =>
    await db.Perfumes.ToListAsync());

app.MapGet("/perfumes/{id:int}", async (int id, AppDbContext db) =>
{
    var perfume = await db.Perfumes.FindAsync(id);

    return perfume is null
        ? Results.NotFound()
        : Results.Ok(perfume);
});

app.Run();