using JourneyMate.Domain.Common.Interfaces;
using JourneyMate.Domain.Entities;

namespace JourneyMate.Domain.Repositories;

public interface IAddressRepository
{
	Task<IPaginatedList<Address>> GetAll(int pageNumber, int pageSize);
	Task<Address> GetByIdAsync(Guid id);
	Task<Address> GetByPlaceIdAsync(string placeId);
	Task AddAsync(Address address);
	Task UpdateAsync(Address address);
	Task DeleteAsync(Address address);
}