using CityInfo.API.DbContext;
using CityInfo.API.Entities;
using Microsoft.EntityFrameworkCore;

namespace CityInfo.API.services;

public class CityInfoRepository : ICityInfoRepository
{
    private readonly CityInfoContext _context;

    public CityInfoRepository(CityInfoContext context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }

    /// <summary>
    /// Retrieves all cities in the database ordered by Name.
    /// </summary>
    /// <returns>
    /// Returns a collection of City objects representing all cities in the database
    /// ordered by Name.
    /// </returns>
    public async Task<IEnumerable<City>> GetCitiesAsync()
    {
        return await _context.Cities.OrderBy(c => c.Name).ToListAsync();
    }

    /// <summary>
    /// Retrieves all cities in the database ordered by Name.
    /// </summary>
    /// <param name="name">Optional. The name of the city to filter by. If provided, only cities with matching names
    /// will be returned.</param>
    /// <param name="searchQuery">Optional. The search query to filter by. If provided, cities with names or
    /// descriptions containing the search query will be returned.</param>
    /// <returns>
    /// Returns a collection of City objects representing all cities in the database
    /// ordered by Name. If the name and searchQuery parameters are provided, the
    /// returned collection will be filtered accordingly.
    /// </returns>
    public async Task<(IEnumerable<City>, PaginationMetadata)> GetCitiesAsync(string? name, string? searchQuery,
        int pageNumber, int pageSize)
    {
        // collection to start from 
        var collection = _context.Cities as IQueryable<City>;

        if (!string.IsNullOrEmpty(name))
        {
            name = name.Trim();
            collection = collection.Where(c => c.Name == name);
        }

        if (!string.IsNullOrEmpty(searchQuery))
        {
            searchQuery = searchQuery.Trim();
            collection = collection.Where(a => a.Name.Contains(searchQuery)
                                               || (a.Description != null && a.Description.Contains(searchQuery)));
        }
        
        var totalItemCount = await collection.CountAsync();
        var paginationMetadata = new PaginationMetadata(totalItemCount, pageSize, pageNumber);

        var totalPageCount = (int)Math.Ceiling((double)totalItemCount / pageSize);

        
        var collectionToReturn =  await collection.OrderBy(c => c.Name)
            .Skip(pageSize * (pageNumber - 1))
            .Take(pageSize)
            .ToListAsync();

        return (collectionToReturn, paginationMetadata);
    }

    /// <summary>
    /// Retrieves a specific city from the database.
    /// </summary>
    /// <param name="cityId">The ID of the city to retrieve.</param>
    /// <param name="includePointsOfInterest">A flag indicating whether to include the city's points of interest.</param>
    /// <returns>    
    /// Returns a City object representing the specific city with the given ID.
    /// If includePointsOfInterest is true, the City object will also include the associated points of interest.
    /// If no city with the given ID is found, returns null.
    /// </returns>
    public async Task<City?> GetCityAsync(int cityId, bool includePointsOfInterest)
    {
        if (includePointsOfInterest)
        {
            return await this._context.Cities.Include(c => c.PointsOfInterest)
                .Where(c => c.Id == cityId).FirstOrDefaultAsync();
        }

        return await _context.Cities.Where(c => c.Id == cityId).FirstOrDefaultAsync();
    }

    /// <summary>
    /// Checks if a city with the given ID exists in the database.
    /// </summary>
    /// <param name="cityId">The ID of the city to check.</param>
    /// <returns>
    /// Returns true if a city with the given ID exists in the database, otherwise returns false.
    /// </returns>
    public async Task<bool> CityExistsAsync(int cityId)
    {
        return await _context.Cities.AnyAsync(c => c.Id == cityId);
    }

    /// <summary>
    /// Retrieves a specific point of interest from the database.
    /// </summary>
    /// <param name="cityId">The ID of the city containing the point of interest.</param>
    /// <param name="pointOfInterestId">The ID of the point of interest to retrieve.</param>
    /// <returns>
    /// Returns a PointOfInterest object representing the specific point of interest with the given ID.
    /// If no point of interest with the given ID is found in the city, returns null.
    /// </returns>
    public async Task<PointOfInterest?> GetPointOfInterestForCityAsync(int cityId, int pointOfInterestId)
    {
        return await _context.PointsOfInterest
            .Where(p => p.CityId == cityId && p.Id == pointOfInterestId)
            .FirstOrDefaultAsync();
    }


    /// <summary>
    /// Retrieves all points of interest for a specific city from the database.
    /// </summary>
    /// <param name="cityId">The ID of the city for which to retrieve points of interest.</param>
    /// <returns>
    /// Returns a collection of PointOfInterest objects representing all points of interest
    /// associated with the specified city.
    /// </returns>
    public async Task<IEnumerable<PointOfInterest>> GetPointsOfInterestForCityAsync(int cityId)
    {
        return await _context.PointsOfInterest
            .Where(p => p.CityId == cityId).ToListAsync();
    }


    /// <summary>
    /// Adds a point of interest for a specific city in the database.
    /// </summary>
    /// <param name="cityId">The ID of the city to add the point of interest for.</param>
    /// <param name="pointOfInterest">The PointOfInterest object representing the point of interest to be added.</param>
    /// <returns>A Task representing the asynchronous operation.</returns>
    public async Task AddPointOfInterestForCityAsync(int cityId, PointOfInterest pointOfInterest)
    {
        var city = await GetCityAsync(cityId, false);
        if (city != null)
        {
            city.PointsOfInterest.Add(pointOfInterest);
        }
    }

    /// <summary>
    /// Deletes a point of interest from the database.
    /// </summary>
    /// <param name="pointOfInterest">The point of interest to delete.</param>
    public void DeletePointOfInterest(PointOfInterest pointOfInterest)
    {
        _context.PointsOfInterest.Remove(pointOfInterest);
    }

    /// <summary>
    /// Saves all changes made in the context to the underlying data storage.
    /// </summary>
    /// <returns>
    /// Returns a boolean value indicating whether the changes were successfully saved or not.
    /// </returns>
    public async Task<bool> SaveChangesAsync()
    {
        return (await _context.SaveChangesAsync() >= 0);
    }
}