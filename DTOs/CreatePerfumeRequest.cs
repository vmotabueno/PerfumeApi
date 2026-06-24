namespace PerfumeApi.DTOs;

public class CreatePerfumeRequest
{
    public string Nome { get; set; } = string.Empty;
    public string Marca { get; set; } = string.Empty;
    public string ImageUrl { get; set; } = string.Empty;
}