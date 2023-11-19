using JourneyMate.Domain.Common;
using JourneyMate.Domain.ValueObjects;

namespace JourneyMate.Domain.Entities;

public class Address : BaseEntity
{
	public string ApiPlaceId { get; private set; }
	public Location Location { get; private set; }
	public string Locality { get; private set; }
	public string AdministrativeAreaLevel2 { get; private set; }
	public string AdministrativeAreaLevel1 { get; private set; }
	public string Country { get; private set; }
	public string PostalCode { get; private set; }

	public List<PlaceAddress> Places { get; private set; } = new();
	private Address() { }

	public Address(string apiPlaceId, Location location, string locality, string administrativeAreaLevel2, string administrativeAreaLevel1, string country, string postalCode)
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