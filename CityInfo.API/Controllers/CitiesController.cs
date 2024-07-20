using CityInfo.API.Models;
using Microsoft.AspNetCore.Mvc;

namespace CityInfo.API.Controllers;

[ApiController]
[Route("api/cities")]
public class CitiesController : ControllerBase
{
    [HttpGet]
    public ActionResult<IEnumerable<CityDto>> GetCites()
    {
        return Ok(CitiesDataStore.Current.Cities);
    }

    [HttpGet("{id}")]
    public ActionResult<CityDto> GetCity(int id)
    {
        var cityToReturn =
            CitiesDataStore.Current.Cities.FirstOrDefault(c => c.Id == id);

        if (cityToReturn == null)
        {
            return NotFound();
        }
        
        return Ok(cityToReturn);
    }
    
    // [HttpPost]
    // public IActionResult CreateCity([FromBody] CityDto city)
    // {
    //     // Generate a new unique ID for the city
    //     var newCityId = CitiesDataStore.Current.Cities.Max(c => c.Id) + 1;
    //     city.Id = newCityId;
    //
    //     // Add the new city to the data store
    //     CitiesDataStore.Current.Cities.Add(city);
    //
    //     // Return the created city with the assigned ID
    //     return CreatedAtAction(nameof(GetCity), new { id = city.Id }, city);
    // }
}