using JourneyMate.Domain.Common;
using JourneyMate.Domain.ValueObjects;

namespace JourneyMate.Domain.Entities;

public class Place : BaseAuditableEntity
{
	public string ApiPlaceId { get; private set; }
	public string BusinessStatus { get; private set; }
	public string Name { get; private set; }
	public double Rating { get; private set; }
	public int UserRatingsTotal { get; private set; }
	public string Vicinity { get; private set; }
	public Location Location { get; private set; }
	public PlusCode PlusCode { get; private set; }
	public Photo? Photo { get; private set; }
	public List<PlaceAddress> Addresses { get; private set; } = new();
	public List<PlaceType> Types { get; private set; } = new();
	
	private Place() { }

	public Place(
		string apiPlaceId,
		string businessStatus,
		string name,
		double rating,
		int userRatingsTotal,
		string vicinity,
		Location location,
		PlusCode plusCode,
		Photo? photo)
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

	public void UpdateLocation(string vicinity, Location location, PlusCode plusCode)
	{
		Vicinity = vicinity;
		Location = location;
		PlusCode = plusCode;
	}

	public void UpdatePhoto(Photo? photo)
	{
		Photo = photo;
	}

	public bool CheckType(List<PlaceType> placeTypes)
	{
		return Types.Any(x => placeTypes.Contains(x));
	}
	
	public void SetTypes(List<PlaceType> placeTypes)
	{
		Types = placeTypes;
	}

	public void AddType(PlaceType placeType)
	{
		if (Types.Contains(placeType)) return;
		Types.Add(placeType);
	}
	
	public bool CheckAddress(Guid addressId)
	{
		return Addresses.Any(x => x.AddressId == addressId);
	}
	
	public void AddAddress(PlaceAddress address)
	{
		if (Addresses.Contains(address)) return;
		Addresses.Add(address);
	}
}