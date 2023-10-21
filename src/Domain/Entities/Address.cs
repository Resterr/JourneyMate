using JourneyMate.Domain.Common;
using JourneyMate.Domain.ValueObjects;

namespace JourneyMate.Domain.Entities;

public class Address : BaseEntity
{
	public string PlaceId { get; set; }
	public Location Location { get; set; }
	public AddressComponent Locality { get; set; }
	public AddressComponent AdministrativeAreaLevel2 { get; set; }
	public AddressComponent AdministrativeAreaLevel1 { get; set; }
	public AddressComponent Country { get; set; }
	public AddressComponent PostalCode { get; set; }
	private Address() { }

	public Address(
		string placeId, Location location, AddressComponent locality, AddressComponent administrativeAreaLevel2, AddressComponent administrativeAreaLevel1,
		AddressComponent country, AddressComponent postalCode)
	{
		PlaceId = placeId;
		Location = location;
		Locality = locality;
		AdministrativeAreaLevel2 = administrativeAreaLevel2;
		AdministrativeAreaLevel1 = administrativeAreaLevel1;
		Country = country;
		PostalCode = postalCode;
	}
}