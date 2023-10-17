using JourneyMate.Application.Common.Models.ReadModels;

namespace JourneyMate.Application.Common.Interfaces;
public interface IGeocodeApiService
{
	Task<AddressReadModel?> GetAddressAsync(string components);
}