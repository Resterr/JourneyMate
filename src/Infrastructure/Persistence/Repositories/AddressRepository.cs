using JourneyMate.Application.Common.Exceptions;
using JourneyMate.Application.Common.Mappings;
using JourneyMate.Domain.Common.Interfaces;
using JourneyMate.Domain.Entities;
using JourneyMate.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace JourneyMate.Infrastructure.Persistence.Repositories;
internal sealed class AddressRepository : IAddressRepository
{
	private readonly ApplicationDbContext _dbContext;

	public AddressRepository(ApplicationDbContext dbContext)
    {
		_dbContext = dbContext;
	}

	public async Task<IPaginatedList<Address>> GetAll(int pageNumber, int pageSize)
	{
		var query = await _dbContext.Addresses
			.OrderBy(x => x.Locality.LongName)
			.PaginatedListAsync(pageNumber, pageSize);

		return query;
	}

	public async Task<Address> GetByIdAsync(Guid id)
	{
		var query = await _dbContext.Addresses.SingleOrDefaultAsync(x => x.Id == id) ?? throw new AddressNotFound(id);
		return query;
	}

	public async Task<Address> GetByPlaceIdAsync(string placeId)
	{
		var query = await _dbContext.Addresses.SingleOrDefaultAsync(x => x.PlaceId == placeId) ?? throw new AddressNotFound(placeId, "place id");
		return query;
	}

	public async Task AddAsync(Address address)
	{
		await _dbContext.Addresses.AddAsync(address);
		await _dbContext.SaveChangesAsync();
	}

	public async Task UpdateAsync(Address address)
	{
		_dbContext.Addresses.Update(address);
		await _dbContext.SaveChangesAsync();
	}

	public async Task DeleteAsync(Address address)
	{
		_dbContext.Addresses.Remove(address);
		await _dbContext.SaveChangesAsync();
	}
}
