using JourneyMate.Application.Common.Mappings;
using JourneyMate.Domain.Entities;
using JourneyMate.Domain.ValueObjects;

namespace JourneyMate.Application.Common.Models;

public class AddressDto : IMapFrom<Address>
{
	public Guid Id { get; set; }
	public string PlaceId { get; set; }
	public Location Location { get; set; }
	public AddressComponent Locality { get; set; }
	public AddressComponent AdministrativeAreaLevel2 { get; set; }
	public AddressComponent AdministrativeAreaLevel1 { get; set; }
	public AddressComponent Country { get; set; }
	public AddressComponent PostalCode { get; set; }
}