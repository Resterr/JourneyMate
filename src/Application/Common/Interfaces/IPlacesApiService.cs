using JourneyMate.Application.Common.Models;

namespace JourneyMate.Application.Common.Interfaces;

public interface IPlacesApiService
{
	Task<List<PlaceAddDto>> GetPlacesAsync(string location, string radius, string type, string rankBy, double latitude, double longitude);
	Task<Stream> LoadPhoto(string photoReference, int height, int width);
}