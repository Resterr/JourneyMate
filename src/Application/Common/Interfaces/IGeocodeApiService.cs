using JourneyMate.Application.Common.Models;

namespace JourneyMate.Application.Common.Interfaces;

public interface IGeocodeApiService
{
	Task<AddressDto?> GetAddressAsync(string components);
}