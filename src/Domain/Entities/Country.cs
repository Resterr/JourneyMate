using JourneyMate.Domain.Common;

namespace JourneyMate.Domain.Entities;

public class Country : BaseEntity
{
	public string ShortName { get; private set; }
	public string LongName { get; private set; }

	private Country() { }

	public Country(string shortName, string longName)
	{
		ShortName = shortName;
		LongName = longName;
	}
}