using JourneyMate.Domain.Common;

namespace JourneyMate.Domain.Entities;

public class Plan : BaseAuditableEntity
{
	public Guid UserId { get; private set; }
	public User User { get; private set; }
	public string Name { get; private set; }
	public List<PlacePlan> Places { get; private set; } = new();
	
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

	public void AddPlaces(List<PlacePlan> places)
	{
		var placesToAdd = places.Where(x => !Places.Contains(x));
		Places.AddRange(placesToAdd);
	}
	
	public void RemovePlaces(List<PlacePlan> places)
	{
		var placesToRemove = places.Where(x => Places.Contains(x));
		
		foreach (var place in placesToRemove)
		{
			Places.Remove(place);
		}
	}
}