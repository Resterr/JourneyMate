namespace JourneyMate.Application.Common.Models;

public class PlanScheduleDto
{
	public Guid PlaceId { get; set; }
	public string PlaceName { get; set; }
	public Guid PlanId { get; set; }
	public string PlanName { get; set; }
	public DateTime? StartingDate { get; set; }
	public DateTime? EndingDate { get; set; }

	/*public PlanScheduleDto(Guid placeId, string placeName, Guid planId, string planName, DateTime? startingDate, DateTime? endingDate)
	{
		PlaceId = placeId;
		PlaceName = placeName;
		PlanId = planId;
		PlanName = planName;
		StartingDate = startingDate;
		EndingDate = endingDate;
	}*/
}