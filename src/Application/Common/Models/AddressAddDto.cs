using JourneyMate.Domain.ValueObjects;

namespace JourneyMate.Application.Common.Models;

public class AddressAddDto
{
	public Guid Id { get; set; }
	public string ApiPlaceId { get; set; }
	public Location Location { get; set; }
	public AddressComponent Locality { get; set; }
	public AdministativeAreaAddDto AdministrativeArea { get; set; }
	public string PostalCode { get; set; }
}