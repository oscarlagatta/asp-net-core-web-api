using AutoMapper;
using CityInfo.API.Models;
using CityInfo.API.services;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;

namespace CityInfo.API.Controllers;

/// <summary>
/// Represents a controller for managing cities in the CityInfo API.
/// </summary>
[ApiController]
[Authorize]
[Route("api/v{version:apiVersion}/cities")]
[ApiVersion(1)]
[ApiVersion(2)]
public class CitiesController : ControllerBase
{
    private readonly ICityInfoRepository _cityInfoRepository;
    private readonly IMapper _mapper;

    private const int maxCitiesPageAize = 20;

    public CitiesController(ICityInfoRepository cityInfoRepository, IMapper mapper)
    {
        _cityInfoRepository = cityInfoRepository ?? throw new ArgumentNullException(nameof(cityInfoRepository));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
    }


    /// <summary>
    /// Retrieves a list of cities from the repository based on the specified criteria.
    /// </summary>
    /// <param name="name">Optional. The name of the city to search for.</param>
    /// <param name="searchQuery">Optional. The search query to filter cities.</param>
    /// <param name="pageNumber">Optional. The page number of the results to retrieve. Default is 1.</param>
    /// <param name="pageSize">Optional. The maximum number of cities per page. Default is 10.</param>
    /// <returns>An asynchronous task that represents the operation. The task result contains the ActionResult with the list of cities.</returns>
    [HttpGet]
    public async Task<ActionResult<IEnumerable<CityWithoutPointsOfInterestDto>>> GetCites(
        string? name, string? searchQuery, int pageNumber = 1, int pageSize = 10)
    {
        if (pageSize > maxCitiesPageAize)
        {
            pageSize = maxCitiesPageAize;
        }

        var (cityEntities, paginationMetadata) = await _cityInfoRepository
            .GetCitiesAsync(name, searchQuery, pageNumber, pageSize);

        Response.Headers.Add("X-Pagination",
            JsonSerializer.Serialize(paginationMetadata));


        return Ok(_mapper.Map<IEnumerable<CityWithoutPointsOfInterestDto>>(cityEntities));
    }

    /// <summary>
    /// Retrieves a city from the repository by its ID.
    /// </summary>
    /// <param name="id">The ID of the city to retrieve.</param>
    /// <param name="includePointsOfInterest">Optional. If set to true, includes the points of interest for the city. Default is false.</param>
    /// <returns>An asynchronous task that represents the operation. The task result contains the ActionResult with the city or not found response.</returns>
    /// <response code="200">Returns the requested City</response>
    /// <response code="404">City Not Found</response>
    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetCity(int id, bool includePointsOfInterest = false)
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