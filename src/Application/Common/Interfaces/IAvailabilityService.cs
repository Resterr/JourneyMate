namespace JourneyMate.Application.Common.Interfaces;

public interface IAvailabilityService
{
	Task<bool> CheckAddress(string placeId);
	Task<bool> CheckUser(string? email, string? userName);
}