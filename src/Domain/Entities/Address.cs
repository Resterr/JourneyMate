using JourneyMate.Domain.Common;
using JourneyMate.Domain.ValueObjects;

namespace JourneyMate.Domain.Entities;

public class Address : BaseEntity
{
	public string ApiPlaceId { get; private set; }
	public AddressComponent Locality { get; private set; }
	public Location Location { get; private set; }
	public Guid AdministrativeAreaId { get; private set; }
	public AdministrativeArea AdministrativeArea { get; private set; }
	public string PostalCode { get; private set; }
	public List<PlaceAddressRelation> Places { get; private set; } = new();
	
	private Address() { }
	public Address(string apiPlaceId, AddressComponent locality, Location location, AdministrativeArea administrativeArea, Country country, string postalCode)
	{
		ApiPlaceId = apiPlaceId;
		Locality = locality;
		Location = location;
		AdministrativeArea = administrativeArea;
		PostalCode = postalCode;
	}
}