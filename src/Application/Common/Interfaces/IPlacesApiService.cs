using JourneyMate.Application.Common.Models;

namespace JourneyMate.Application.Common.Interfaces;

public interface IPlacesApiService
{
	Task<List<PlaceDto>> GetPlacesAsync(string location, string radius, string type, string rankBy, double latitude, double longitude);
}