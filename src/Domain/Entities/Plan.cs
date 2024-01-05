using JourneyMate.Domain.Common;

namespace JourneyMate.Domain.Entities;

public class Plan : BaseAuditableEntity
{
	public Guid UserId { get; set; }
	public User User { get; private set; }
	public string Name { get; private set; }
	public List<Place> Places { get; } = new();
	public List<FollowPlanRelation> Shared { get; } = new();

	private Plan() { }

	public Plan(User user, string name)
	{
		User = user;
		Name = name;
	}

	public void Update(string name)
	{
		Name = name;
	}

	public void AddPlaces(List<Place> places)
	{
		var placesToAdd = places.Where(x => !Places.Contains(x));

		foreach (var placeToAdd in placesToAdd) Places.Add(placeToAdd);
	}

	public void RemovePlaces(List<Place> places)
	{
		var placesToRemove = places.Where(x => Places.Contains(x));

		foreach (var place in placesToRemove) Places.Remove(place);
	}

	public void AddShare(FollowPlanRelation share)
	{
		if (Shared.Contains(share)) return;
		Shared.Add(share);
	}
}