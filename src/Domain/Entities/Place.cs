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
	public List<Photo> Photos { get; } = new();
	public List<PlaceAddressRelation> Addresses { get; } = new();
	public List<PlaceType> Types { get; private set; } = new();
	public List<Report> Reports { get; private set; } = new();
	public List<Plan> Plans { get; private set; } = new();

	private Place() { }

	public Place(string apiPlaceId, string businessStatus, string name, double rating, int userRatingsTotal, string vicinity, Location location, PlusCode plusCode)
	{
		ApiPlaceId = apiPlaceId;
		BusinessStatus = businessStatus;
		Name = name;
		Rating = rating;
		UserRatingsTotal = userRatingsTotal;
		Vicinity = vicinity;
		Location = location;
		PlusCode = plusCode;
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

	public bool CheckType(List<PlaceType> placeTypes)
	{
		var hasType = false;
		foreach (var placeType in placeTypes)
			if (Types.Any(x => x.Id == placeType.Id))
				hasType = true;

		return hasType;
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

	public void AddPhoto(Photo photo)
	{
		Photos.Add(photo);
	}

	public bool CheckAddress(Guid addressId)
	{
		return Addresses.Any(x => x.AddressId == addressId);
	}

	public void AddAddress(PlaceAddressRelation addressRelation)
	{
		if (Addresses.Contains(addressRelation)) return;
		Addresses.Add(addressRelation);
	}
}