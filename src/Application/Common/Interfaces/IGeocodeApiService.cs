using JourneyMate.Application.Common.Models;

namespace JourneyMate.Application.Common.Interfaces;

public interface IGeocodeApiService
{
	Task<AddressAddDto?> GetAddressAsync(string components, string? localityName);
	Task<AddressAddDto?> GetAddressFromTerytAsync(string county, string municipality, bool isCity);
}