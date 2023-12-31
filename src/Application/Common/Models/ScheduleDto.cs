namespace JourneyMate.Application.Common.Models;

public class ScheduleDto
{
	public Guid Id { get; set; }
	public Guid PlaceId { get; set; }
	public string PlaceName { get; set; }
	public Guid PlanId { get; set; }
	public string PlanName { get; set; }
	public DateTime StartingDate { get; set; }
	public DateTime? EndingDate { get; set; }
}