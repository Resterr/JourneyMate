using JourneyMate.Domain.Common;

namespace JourneyMate.Domain.ValueObjects;

public class Location : ValueObject
{
	public double Latitude { get; set; }
	public double Longitude { get; set; }

	public Location(double latitude, double longitude)
	{
		Latitude = latitude;
		Longitude = longitude;
	}

	protected override IEnumerable<object> GetEqualityComponents()
	{
		yield return Latitude;
		yield return Longitude;
	}
}