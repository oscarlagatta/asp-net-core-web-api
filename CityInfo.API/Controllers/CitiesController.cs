using AutoMapper;
using CityInfo.API.Models;
using CityInfo.API.services;
using Microsoft.AspNetCore.Mvc;

namespace CityInfo.API.Controllers;

[ApiController]
[Route("api/cities")]
public class CitiesController : ControllerBase
{
    private readonly ICityInfoRepository _cityInfoRepository;
    private readonly IMapper _mapper;

    public CitiesController(ICityInfoRepository cityInfoRepository, IMapper mapper)
    {
        _cityInfoRepository = cityInfoRepository ?? throw new ArgumentNullException(nameof(cityInfoRepository));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
    }

    /// <summary>
    /// Retrieves all cities without their points of interest from the repository.
    /// </summary>
    /// <returns>An asynchronous task that represents the operation. The task result contains the list of cities without points of interest.</returns>
    [HttpGet]
    public async Task<ActionResult<IEnumerable<CityWithoutPointsOfInterestDto>>> GetCites()
    {
        var cityEntities = await _cityInfoRepository.GetCitiesAsync();

        return Ok(_mapper.Map<IEnumerable<CityWithoutPointsOfInterestDto>>(cityEntities));
    }

    /// <summary>
    /// Retrieves a city from the repository by its ID.
    /// </summary>
    /// <param name="id">The ID of the city to retrieve.</param>
    /// <param name="includePointsOfInterest">Optional. If set to true, includes the points of interest for the city. Default is false.</param>
    /// <returns>An asynchronous task that represents the operation. The task result contains the ActionResult with the city or not found response.</returns>
    [HttpGet("{id}")]
    public async  Task<IActionResult> GetCity(int id, bool includePointsOfInterest = false)
    {

        var city = await _cityInfoRepository.GetCityAsync(id, includePointsOfInterest);

        if (city == null)
        {
            return NotFound();
        }

        if (includePointsOfInterest)
        {
            return Ok(_mapper.Map<CityDto>(city));    
        }

        return Ok(_mapper.Map<CityWithoutPointsOfInterestDto>(city));
    }
}