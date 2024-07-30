namespace CityInfo.API.Models;

/// <summary>
/// Represents a point of interest data transfer object (DTO).
/// </summary>
public class PointOfInterestDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
}