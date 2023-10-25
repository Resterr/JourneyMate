using JourneyMate.Domain.Common.Interfaces;
using JourneyMate.Domain.Entities;

namespace JourneyMate.Domain.Repositories;

public interface IPlaceRepository
{
	Task<IPaginatedList<Place>> GetAllAsync(Guid addressId, int pageNumber, int pageSize);
	Task<Place> GetByIdAsync(Guid id);
	Task<Place> GetByPlaceIdAsync(string placeId);
	Task AddRangeAsync(List<Place> place);
	Task UpdateRangeAsync(List<Place> place);
	Task DeleteRangeAsync(List<Place> place);
}