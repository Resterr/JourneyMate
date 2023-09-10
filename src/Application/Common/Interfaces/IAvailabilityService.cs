namespace JourneyMate.Application.Common.Interfaces;
public interface IAvailabilityService
{
	Task<bool> CheckAddress(string placeId);
}