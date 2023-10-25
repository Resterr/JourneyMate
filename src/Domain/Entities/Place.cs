using JourneyMate.Domain.Common;
using JourneyMate.Domain.ValueObjects;

namespace JourneyMate.Domain.Entities;

public class Place : BaseAuditableEntity
{
	public string ApiPlaceId { get; private set; }
	public Guid AddressId { get; private set; }
	public Address Address { get; }
	public string BusinessStatus { get; private set; }
	public string Name { get; private set; }
	public double Rating { get; private set; }
	public int UserRatingsTotal { get; private set; }
	public string Vicinity { get; private set; }
	public double DistanceFromAddress { get; private set; }
	public Location Location { get; private set; }
	public PlusCode PlusCode { get; private set; }
	public Photo? Photo { get; private set; }
	public List<PlaceType> Types { get; private set; } = new();

	private Place() { }

	public Place(
		string apiPlaceId,
		string businessStatus,
		string name,
		double rating,
		int userRatingsTotal,
		string vicinity,
		double distanceFromAddress,
		Location location,
		PlusCode plusCode,
		Photo? photo,
		Guid addressId)
	{
		ApiPlaceId = apiPlaceId;
		BusinessStatus = businessStatus;
		Name = name;
		Rating = rating;
		UserRatingsTotal = userRatingsTotal;
		Vicinity = vicinity;
		Location = location;
		PlusCode = plusCode;
		Photo = photo;
		AddressId = addressId;
		DistanceFromAddress = distanceFromAddress;
	}

	public void UpdateRatings(double rating, int userRatingsTotal)
	{
		Rating = rating;
		UserRatingsTotal = userRatingsTotal;
	}

	public void UpdateInfo(string businessStatus, string name)
	{
		BusinessStatus = businessStatus;
		Name = name;
	}

	public void UpdateLocation(string vicinity, Location location, PlusCode plusCode, double distanceFromAddress)
	{
		Vicinity = vicinity;
		Location = location;
		PlusCode = plusCode;
		DistanceFromAddress = distanceFromAddress;
	}

	public void UpdatePhoto(Photo? photo)
	{
		Photo = photo;
	}

	public void SetTypes(List<PlaceType> placeTypes)
	{
		Types = placeTypes;
		//Types.AddRange(placeTypes);
	}

	public void AddType(PlaceType placeType)
	{
		if (Types.Contains(placeType)) return;
		Types.Add(placeType);
	}

	public void RemoveType(PlaceType placeType)
	{
		if (!Types.Contains(placeType)) return;
		Types.Remove(placeType);
	}
}