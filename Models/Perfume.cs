using System.ComponentModel.DataAnnotations;

namespace PerfumeApi.Models;

public class Perfume
{
    public int Id { get; set; }

    [Required]
    [MaxLength(120)]
    public string Nome { get; set; } = string.Empty;

    [Required]
    [MaxLength(120)]
    public string Marca { get; set; } = string.Empty;

    [Required]
    [MaxLength(500)]
     public string ImageUrl { get; set; } = string.Empty;
}