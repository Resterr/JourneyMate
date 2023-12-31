using JourneyMate.Domain.ValueObjects;

namespace JourneyMate.Application.Common.Models;

public class AdministativeAreaAddDto
{
	public AddressComponent Level1 { get; set; }
	public AddressComponent Level2 { get; set; }
	public AddressComponent Country { get; set; }

	public AdministativeAreaAddDto(AddressComponent level1, AddressComponent level2, AddressComponent country)
	{
		Level1 = level1;
		Level2 = level2;
		Country = country;
	}
}