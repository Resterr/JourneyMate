using JourneyMate.Application.Common.Mappings;
using JourneyMate.Domain.Entities;
using JourneyMate.Domain.ValueObjects;

namespace JourneyMate.Application.Common.Models;

public class AddressDto
{
	public Guid Id { get; set; }
	public string ApiPlaceId { get; set; }
	public Location Location { get; set; }
	public string Locality { get; set; }
	public string AdministrativeAreaLevel2 { get; set; }
	public string AdministrativeAreaLevel1 { get; set; }
	public string Country { get; set; }
	public string PostalCode { get; set; }
}