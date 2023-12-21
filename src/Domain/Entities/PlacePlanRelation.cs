namespace JourneyMate.Domain.Entities;

public class PlacePlanRelation
{
	public Guid PlaceId { get; private set; }
	public Place Place { get; private set; }
	public Guid PlanId { get; private set; }
	public Plan Plan { get; private set; }
	public DateTime? StartingDate { get; private set; }
	public DateTime? EndingDate { get; private set; }

	private PlacePlanRelation() { }
	public PlacePlanRelation(Place place, Plan plan)
	{
		Place = place;
		Plan = plan;
	}

	public void UpdateSchedule(DateTime? startingDate, DateTime? endingDate)
	{
		StartingDate = startingDate;
		EndingDate = endingDate;
	}
}