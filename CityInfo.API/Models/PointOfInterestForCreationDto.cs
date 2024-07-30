using System.ComponentModel.DataAnnotations;

namespace CityInfo.API.Models;

/// <summary>
/// Represents a point of interest data transfer object for creation.
/// </summary>
public class PointOfInterestForCreationDto
{
    [Required(ErrorMessage = "You should provide a Name value")]
    [MaxLength(50)]
    public string Name { get; set; } = string.Empty;
    
    [MaxLength(200)]
    public string? Description { get; set; }
}