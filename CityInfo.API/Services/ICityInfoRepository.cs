using CityInfo.API.Entities;

namespace CityInfo.API.services;

public interface ICityInfoRepository
{
    Task<IEnumerable<City>> GetCitiesAsync();
    
    Task<City?> GetCityAsync(int cityId, bool includePointsOfInterest);
    
    Task<IEnumerable<PointOfInterest>> GetPointsOfInterestForCityAsync(int cityId);
    
    Task<PointOfInterest?> GetPointOfInterestForCityAsync(int cityId, int pointOfInterestId);

    Task<bool> CityExistsAsync(int cityId);

    Task AddPointOfInterestForCityAsync(int cityId, PointOfInterest pointOfInterest);

    void DeletePointOfInterest(PointOfInterest pointOfInterest);
    
    Task<bool> SaveChangesAsync();
}