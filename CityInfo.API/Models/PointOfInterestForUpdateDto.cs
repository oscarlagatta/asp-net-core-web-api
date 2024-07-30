using System.ComponentModel.DataAnnotations;

namespace CityInfo.API.Models;

/// <summary>
/// DTO class for updating a point of interest.
/// </summary>
public class PointOfInterestForUpdateDto
{
    [Required(ErrorMessage = "You should provide a name value.")]
    [MaxLength(50)]
    public string Name { get; set; } = string.Empty;

    [MaxLength(200)]
    public string? Description { get; set; }
}