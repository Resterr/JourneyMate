using JourneyMate.Domain.Common;

namespace JourneyMate.Domain.Entities;

public class AdministrativeAreaLevel1 : BaseEntity
{
	public string ShortName { get; private set; }
	public string LongName { get; private set; }

	public Guid CountryId { get; }
	public Country Country { get; private set; }

	private AdministrativeAreaLevel1() { }

	public AdministrativeAreaLevel1(string shortName, string longName, Country country)
	{
		ShortName = shortName;
		LongName = longName;
		Country = country;
	}
}