using AutoMapper;
using CityInfo.API.Entities;
using CityInfo.API.Models;
using CityInfo.API.services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;

namespace CityInfo.API.Controllers;

[Route("api/cities/{cityId}/pointsofinterest")]
[Authorize(Policy = "MustBeFromLondon")]
[ApiController]
public class PointsOfInterestController : ControllerBase
{
    private readonly ILogger<PointsOfInterestController> _logger;
    private readonly IMailService _mailService;
    private readonly ICityInfoRepository _cityInfoRepository;
    private readonly IMapper _mapper;


    /// <summary>
    /// Controller that handles API routes related to points of interest for cities.
    /// </summary>
    public PointsOfInterestController(ILogger<PointsOfInterestController> logger, IMailService mailService,
        ICityInfoRepository cityInfoRepository,
        IMapper mapper)
    {
        _logger = logger ??
                  throw new ArgumentNullException(nameof(logger));
        _mailService = mailService ??
                       throw new ArgumentNullException(nameof(mailService));
        _cityInfoRepository = cityInfoRepository
                              ?? throw new ArgumentNullException(nameof(cityInfoRepository));
        _mapper = mapper
                  ?? throw new ArgumentNullException(nameof(mapper));
    }

    /// <summary>
    /// Retrieves the points of interest for a specific city.
    /// </summary>
    /// <param name="cityId">The ID of the city.</param>
    /// <returns>An ActionResult containing a collection of PointOfInterestDto objects.</returns>
    [HttpGet]
    public async Task<ActionResult<IEnumerable<PointOfInterestDto>>> GetPointsOfInterest(int cityId)
    {
        if (!await _cityInfoRepository.CityExistsAsync(cityId))
        {
            _logger.LogInformation(
                $"City with id {cityId} wasn't found when accessing points of interest.");
            return NotFound();
        }

        var pointsOfInterestForCity = await _cityInfoRepository
            .GetPointsOfInterestForCityAsync(cityId);

        return Ok(_mapper.Map<IEnumerable<PointOfInterest>>(pointsOfInterestForCity));
    }

    /// <summary>
    /// Retrieves a specific point of interest for a city.
    /// </summary>
    /// <param name="cityId">The ID of the city.</param>
    /// <param name="pointOfInterestId">The ID of the point of interest.</param>
    /// <returns>An ActionResult containing the PointOfInterestDto object for the specified city and point of interest.</returns>
    [HttpGet("{pointofinterestid}", Name = "GetPointOfInterest")]
    public async Task<ActionResult<PointOfInterestDto>> GetPointOfInterest(
        int cityId, int pointOfInterestId)
    {
        if (!await _cityInfoRepository.CityExistsAsync(cityId))
        {
            return NotFound();
        }

        var pointOfInterest = await _cityInfoRepository.GetPointOfInterestForCityAsync(cityId, pointOfInterestId);
        if (pointOfInterest == null)
        {
            return NotFound();
        }

        return Ok(_mapper.Map<PointOfInterestDto>(pointOfInterest));
    }

    /// <summary>
    /// Creates a new point of interest for a city.
    /// </summary>
    /// <param name="cityId">The ID of the city.</param>
    /// <param name="pointOfInterest">The data of the point of interest to be created.</param>
    /// <returns>An ActionResult of type PointOfInterestDto representing the newly created point of interest.</returns>
    [HttpPost]
    public async Task<ActionResult<PointOfInterestDto>> CreatePointOfInterest(int cityId,
        PointOfInterestForCreationDto pointOfInterest)
    {
        if (!await _cityInfoRepository.CityExistsAsync(cityId))
        {
            return NotFound();
        }

        var finalPointOfInterest = _mapper.Map<PointOfInterest>(pointOfInterest);


        await _cityInfoRepository.AddPointOfInterestForCityAsync(
            cityId, finalPointOfInterest);

        await _cityInfoRepository.SaveChangesAsync();


        var createdPointOfInterestToReturn =
            _mapper.Map<PointOfInterestDto>(finalPointOfInterest);

        return CreatedAtRoute("GetPointOfInterest",
            new
            {
                cityId = cityId,
                pointOfInterestId = createdPointOfInterestToReturn.Id
            },
            createdPointOfInterestToReturn
        );
    }

    /// <summary>
    /// Updates a point of interest for a city.
    /// </summary>
    /// <param name="cityId">The ID of the city.</param>
    /// <param name="pointOfInterestId">The ID of the point of interest.</param>
    /// <param name="pointOfInterest">The updated point of interest details.</param>
    /// <returns>Returns a <see cref="ActionResult"/> indicating the success or failure of the update operation.</returns>
    [HttpPut("{pointofinterestid}")]
    public async Task<ActionResult> UpdatePointOfInterest(int cityId, int pointOfInterestId,
        PointOfInterestForUpdateDto pointOfInterest)
    {
        if (!await _cityInfoRepository.CityExistsAsync(cityId))
        {
            return NotFound();
        }

        // find point of interest 
        var pointOfInterestEntity = await _cityInfoRepository
            .GetPointOfInterestForCityAsync(cityId, pointOfInterestId);

        if (pointOfInterestEntity == null)
        {
            return NotFound();
        }

        _mapper.Map(pointOfInterest, pointOfInterestEntity);

        await _cityInfoRepository.SaveChangesAsync();

        return NoContent();
    }

    /// <summary>
    /// Partially updates a point of interest for a city.
    /// </summary>
    /// <param name="cityId">The ID of the city.</param>
    /// <param name="pointOfInterestId">The ID of the point of interest.</param>
    /// <param name="patchDocument">The JSON patch document containing the updates.</param>
    /// <returns>The updated point of interest. If the city or the point of interest does not exist, returns NotFound().
    /// If the patch document is invalid, returns BadRequest(ModelState). Otherwise, returns NoContent().</returns>
    [HttpPatch("{pointofinterestid}")]
    public async Task<ActionResult> PartiallyUpdatePointOfInterest(int cityId, int pointOfInterestId,
        JsonPatchDocument<PointOfInterestForUpdateDto> patchDocument)
    {
        // check if the city exists
        if (!await _cityInfoRepository.CityExistsAsync(cityId))
        {
            return NotFound();
        }

        // get the point of interest entity 
        var pointOfInterestEntity =
            await _cityInfoRepository.GetPointOfInterestForCityAsync(cityId, pointOfInterestId);

        if (pointOfInterestEntity == null)
        {
            return NotFound();
        }

        var pointOfInterestToPatch = _mapper.Map<PointOfInterestForUpdateDto>(pointOfInterestEntity);

        patchDocument.ApplyTo(pointOfInterestToPatch, ModelState);

        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        if (!TryValidateModel(pointOfInterestToPatch))
        {
            return BadRequest(ModelState);
        }

        _mapper.Map(pointOfInterestToPatch, pointOfInterestEntity);

        await _cityInfoRepository.SaveChangesAsync();

        return NoContent();
    }


    /// <summary>
    /// Deletes a point of interest for a specific city.
    /// </summary>
    /// <param name="cityId">The ID of the city.</param>
    /// <param name="pointOfInterestId">The ID of the point of interest.</param>
    /// <returns>An ActionResult representing the result of the delete operation.</returns>
    [HttpDelete("{pointofinterestid}")]
    public async Task<ActionResult> DeletePointOfInterest(int cityId, int pointOfInterestId)
    {
        if (!await _cityInfoRepository.CityExistsAsync(cityId))
        {
            return NotFound();
        }

        var pointOfInterestEntity =
            await _cityInfoRepository.GetPointOfInterestForCityAsync(cityId, pointOfInterestId);
        if (pointOfInterestEntity == null)
        {
            return NotFound();
        }

        _cityInfoRepository.DeletePointOfInterest(pointOfInterestEntity);

        await _cityInfoRepository.SaveChangesAsync();

        _mailService.Send(
            "Point of interest deleted.",
            $"Point of interest {pointOfInterestEntity.Name} with Id {pointOfInterestEntity.Id} has been deleted!"
        );


        return NoContent();
    }
}