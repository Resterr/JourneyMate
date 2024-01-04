using JourneyMate.Application.Common.Models;

namespace JourneyMate.Application.Common.Interfaces;

public interface IPlacesApiService
{
	Task<List<PlaceAddDto>> GetPlacesAsync(string location, List<string> types, int radius, string rankBy);
	Task<byte[]> LoadPhoto(string photoReference, int height, int width);
}