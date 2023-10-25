using JourneyMate.Application.Common.Exceptions;
using JourneyMate.Application.Common.Mappings;
using JourneyMate.Domain.Common.Interfaces;
using JourneyMate.Domain.Entities;
using JourneyMate.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace JourneyMate.Infrastructure.Persistence.Repositories;

internal sealed class PlaceRepository : IPlaceRepository
{
	private readonly ApplicationDbContext _dbContext;

	public PlaceRepository(ApplicationDbContext dbContext)
	{
		_dbContext = dbContext;
	}

	public async Task<IPaginatedList<Place>> GetAllAsync(Guid addressId, int pageNumber, int pageSize)
	{
		var query = await _dbContext.Places.Where(x => x.AddressId == addressId)
			.OrderByDescending(x => x.Rating)
			.PaginatedListAsync(pageNumber, pageSize);

		return query;
	}

	public async Task<Place> GetByIdAsync(Guid id)
	{
		var query = await _dbContext.Places.SingleOrDefaultAsync(x => x.Id == id) ?? throw new PlaceNotFound(id);

		return query;
	}

	public async Task<Place> GetByPlaceIdAsync(string placeId)
	{
		var query = await _dbContext.Places.SingleOrDefaultAsync(x => x.ApiPlaceId == placeId) ?? throw new PlaceNotFound(placeId, "place id");

		return query;
	}

	public async Task AddRangeAsync(List<Place> places)
	{
		foreach (var place in places)
			if (await _dbContext.Places.AnyAsync(x => x.ApiPlaceId == place.ApiPlaceId) == false)
				await _dbContext.Places.AddAsync(place);

		await _dbContext.SaveChangesAsync();
	}

	public async Task UpdateRangeAsync(List<Place> place)
	{
		_dbContext.Places.UpdateRange(place);
		await _dbContext.SaveChangesAsync();
	}

	public async Task DeleteRangeAsync(List<Place> place)
	{
		_dbContext.Places.RemoveRange(place);
		await _dbContext.SaveChangesAsync();
	}
}