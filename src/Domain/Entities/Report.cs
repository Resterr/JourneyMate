using System.ComponentModel.DataAnnotations.Schema;
using JourneyMate.Domain.Common;

namespace JourneyMate.Domain.Entities;

public class Report : BaseAuditableEntity
{
	public Guid UserId { get; }
	public User User { get; private set; }
	public Guid AddressId { get; }
	public Address Address { get; private set; }
	public int Rating { get; private set; }
	public string Name { get; private set; }
	public List<Place> Places { get; private set; } = new();
	public List<PlaceType> Types { get; private set; } = new();
	[NotMapped]
	public int ReportNumber { get; set; }

	private Report() { }

	public Report(User user, Address address, List<Place> places, List<PlaceType> types)
	{
		User = user;
		Address = address;
		Rating = 0;
		Places = places;
		Types = types;
		ReportNumber = User.Reports.Count + 1;
		UpdateName();
	}
	
	public void UpdateName()
	{
		Name = $"{User.UserName}-{ReportNumber}";
	}
	
	public void UpdateRating(int rating)
	{
		Rating = rating;
	}
}