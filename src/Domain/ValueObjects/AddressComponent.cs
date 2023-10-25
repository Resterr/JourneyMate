using JourneyMate.Domain.Common;

namespace JourneyMate.Domain.ValueObjects;

public class AddressComponent : ValueObject
{
	public string ShortName { get; set; }
	public string LongName { get; set; }

	public AddressComponent(string shortName, string longName)
	{
		ShortName = shortName;
		LongName = longName;
	}

	protected override IEnumerable<object> GetEqualityComponents()
	{
		yield return ShortName;
		yield return LongName;
	}
}