using JourneyMate.Domain.Common;

namespace JourneyMate.Domain.Entities;

public class Schedule : BaseAuditableEntity
{
	public Guid PlanId { get; set; }
	public Plan Plan { get; private set; }
	public Guid PlaceId { get; set; }
	public Place Place { get; private set; }
	public DateTime StartingDate { get; private set; }
	public DateTime? EndingDate { get; private set; }

	private Schedule() { }

	public Schedule(Plan plan, Place place, DateTime startingDate, DateTime? endingDate)
	{
		Plan = plan;
		Place = place;
		StartingDate = startingDate;
		EndingDate = endingDate;
	}

	public void Update(DateTime startingDate, DateTime? endingDate)
	{
		StartingDate = startingDate;
		EndingDate = endingDate;
	}
}