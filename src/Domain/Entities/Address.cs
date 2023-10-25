using JourneyMate.Domain.Common;
using JourneyMate.Domain.ValueObjects;

namespace JourneyMate.Domain.Entities;

public class Address : BaseEntity
{
	public string ApiPlaceId { get; private set; }
	public Location Location { get; private set; }
	public AddressComponent Locality { get; private set; }
	public AddressComponent AdministrativeAreaLevel2 { get; private set; }
	public AddressComponent AdministrativeAreaLevel1 { get; private set; }
	public AddressComponent Country { get; private set; }
	public AddressComponent PostalCode { get; private set; }

	public List<Place> Places { get; private set; } = new();
	private Address() { }

	public Address(
		string apiPlaceId,
		Location location,
		AddressComponent locality,
		AddressComponent administrativeAreaLevel2,
		AddressComponent administrativeAreaLevel1,
		AddressComponent country,
		AddressComponent postalCode)
	{
		ApiPlaceId = apiPlaceId;
		Location = location;
		Locality = locality;
		AdministrativeAreaLevel2 = administrativeAreaLevel2;
		AdministrativeAreaLevel1 = administrativeAreaLevel1;
		Country = country;
		PostalCode = postalCode;
	}
}