using System.ComponentModel.DataAnnotations;

namespace MoviesAPI.Dto;

public class MovieDto
{
    [Required]
    public string Title { get; set; }
    
    [Required]
    public int Year { get; set; }

    public string? Studios { get; set; }
    
    [Required]
    public string? Producers { get; set; }
    
    [Required]
    public bool Winner { get; set; }
}
