using JourneyMate.Domain.Common;
using JourneyMate.Domain.ValueObjects;

namespace JourneyMate.Domain.Entities;

public class AdministrativeArea : BaseEntity
{
	public AddressComponent Level1 { get; private set; }
	public AddressComponent Level2 { get; private set; }
	public Guid CountryId { get; private set; }
	public Country Country { get; private set; }
	
	private AdministrativeArea() { }
	public AdministrativeArea(AddressComponent level1, AddressComponent level2, Country country)
	{
		Level1 = level1;
		Level2 = level2;
		Country = country;
	}
}