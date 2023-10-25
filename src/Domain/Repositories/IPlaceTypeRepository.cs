using JourneyMate.Domain.Entities;

namespace JourneyMate.Domain.Repositories;

public interface IPlaceTypeRepository
{
	Task<List<PlaceType>> GetAllAsync();
	Task<PlaceType> GetByIdAsync(Guid id);
	Task<PlaceType> GetByNameAsync(string name);
	Task AddRangeAsync(List<PlaceType> placeTypes);
	Task UpdateAsync(PlaceType placeType);
	Task DeleteAsync(PlaceType placeType);
}