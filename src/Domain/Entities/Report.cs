using JourneyMate.Domain.Common;

namespace JourneyMate.Domain.Entities;

public class Report : BaseAuditableEntity
{
	public Guid UserId { get; private set; }
	public User User { get; private set; }
	public Guid AddressId { get; private set; }
	public Address Address { get; private set; }
	public int Rating { get; private set; }
	public List<Place> Places { get; private set; } = new();  
	public List<PlaceType> Types { get; private set; } = new();
	
	private Report() { }
	public Report(User user, Address address, List<Place> places, List<PlaceType> types)
	{
		User = user;
		Address = address;
		Rating = 0;
		Places = places;
		Types = types;
	}

	public void UpdateRating(int rating)
	{
		Rating = rating;
	}
}