using JourneyMate.Application.Common.Models.Responses;

namespace JourneyMate.Application.Common.Interfaces;
public interface IGeocodeApiService
{
	Task<AddressResponseDto?> GetAddressAsync(string components);
}