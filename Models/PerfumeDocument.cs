using Google.Cloud.Firestore;

namespace PerfumeApi.Models;

[FirestoreData]
public class PerfumeDocument
{
    [FirestoreDocumentId]
    public string Id { get; set; } = string.Empty;

    [FirestoreProperty]
    public string Nome { get; set; } = string.Empty;

    [FirestoreProperty]
    public string Marca { get; set; } = string.Empty;

    [FirestoreProperty]
    public string ImageUrl { get; set; } = string.Empty;
}
