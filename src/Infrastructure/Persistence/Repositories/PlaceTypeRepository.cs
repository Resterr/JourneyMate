using JourneyMate.Application.Common.Exceptions;
using JourneyMate.Domain.Entities;
using JourneyMate.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace JourneyMate.Infrastructure.Persistence.Repositories;

internal sealed class PlaceTypeRepository : IPlaceTypeRepository
{
	private readonly ApplicationDbContext _dbContext;

	public PlaceTypeRepository(ApplicationDbContext dbContext)
	{
		_dbContext = dbContext;
	}

	public async Task<List<PlaceType>> GetAllAsync()
	{
		var query = await _dbContext.PlaceTypes.ToListAsync();

		return query;
	}

	public async Task<PlaceType> GetByIdAsync(Guid id)
	{
		var query = await _dbContext.PlaceTypes.SingleOrDefaultAsync(x => x.Id == id) ?? throw new PlaceTypeNotFound(id);

		return query;
	}

	public async Task<PlaceType> GetByNameAsync(string name)
	{
		var query = await _dbContext.PlaceTypes.SingleOrDefaultAsync(x => x.Name == name) ?? throw new PlaceTypeNotFound(name);

		return query;
	}

	public async Task AddRangeAsync(List<PlaceType> placeTypes)
	{
		foreach (var placeType in placeTypes)
			if (await _dbContext.PlaceTypes.AnyAsync(x => x.Name == placeType.Name) == false)
				await _dbContext.PlaceTypes.AddAsync(placeType);
		await _dbContext.SaveChangesAsync();
	}

	public async Task UpdateAsync(PlaceType placeType)
	{
		_dbContext.PlaceTypes.Update(placeType);
		await _dbContext.SaveChangesAsync();
	}

	public async Task DeleteAsync(PlaceType placeType)
	{
		_dbContext.PlaceTypes.Remove(placeType);
		await _dbContext.SaveChangesAsync();
	}
}