using Google.Api.Gax;
using Google.Cloud.Firestore;
using PerfumeApi.DTOs;
using PerfumeApi.Models;

var builder = WebApplication.CreateBuilder(args);

var projectId = builder.Configuration["Firestore:ProjectId"]
    ?? throw new InvalidOperationException("Configure Firestore:ProjectId.");

builder.Services.AddFirestoreDb(options =>
{
    options.ProjectId = projectId;
    options.EmulatorDetection = EmulatorDetection.EmulatorOrProduction;
});

var app = builder.Build();

app.MapGet("/", () => "API de Perfumes rodando.");

app.MapPost("/perfumes", async (CreatePerfumeRequest request, FirestoreDb db) =>
{
    if (string.IsNullOrWhiteSpace(request.Nome))
        return Results.BadRequest("O campo Nome é obrigatório.");

    if (string.IsNullOrWhiteSpace(request.Marca))
        return Results.BadRequest("O campo Marca é obrigatório.");

    if (string.IsNullOrWhiteSpace(request.ImageUrl))
        return Results.BadRequest("O campo ImageUrl é obrigatório.");

    var perfumeDocument = new PerfumeDocument
    {
        Nome = request.Nome.Trim(),
        Marca = request.Marca.Trim(),
        ImageUrl = request.ImageUrl.Trim()
    };

    var documentReference = await db.Collection("perfumes").AddAsync(perfumeDocument);
    perfumeDocument.Id = documentReference.Id;

    return Results.Created($"/perfumes/{perfumeDocument.Id}", perfumeDocument);
});

app.MapGet("/perfumes", async (FirestoreDb db) =>
{
    var snapshot = await db.Collection("perfumes").GetSnapshotAsync();
    var perfumes = snapshot.Documents
        .Select(document =>
        {
            var perfume = document.ConvertTo<PerfumeDocument>();
            perfume!.Id = document.Id;
            return perfume;
        })
        .ToList();

    return Results.Ok(perfumes);
});

app.MapGet("/perfumes/{id}", async (string id, FirestoreDb db) =>
{
    var document = await db.Collection("perfumes").Document(id).GetSnapshotAsync();

    if (!document.Exists)
        return Results.NotFound();

    var perfume = document.ConvertTo<PerfumeDocument>();
    if (perfume is null)
        return Results.NotFound();

    perfume.Id = document.Id;
    return Results.Ok(perfume);
});

app.Run();