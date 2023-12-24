using JourneyMate.Domain.Common;
using JourneyMate.Domain.ValueObjects;

namespace JourneyMate.Domain.Entities;

public class Address : BaseEntity
{
	public string ApiPlaceId { get; private set; }
	public AddressComponent Locality { get; private set; }
	public Location Location { get; private set; }
	public Guid AdministrativeAreaLevel2Id { get; private set; }
	public AdministrativeAreaLevel2 AdministrativeAreaLevel2 { get; private set; }
	public string PostalCode { get; private set; }
	public List<PlaceAddressRelation> Places { get; private set; } = new();
	
	private Address() { }
	public Address(string apiPlaceId, AddressComponent locality, Location location, AdministrativeAreaLevel2 administrativeAreaLevel2, string postalCode)
	{
		ApiPlaceId = apiPlaceId;
		Locality = locality;
		Location = location;
		AdministrativeAreaLevel2 = administrativeAreaLevel2;
		PostalCode = postalCode;
	}
}