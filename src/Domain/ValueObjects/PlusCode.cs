using JourneyMate.Domain.Common;

namespace JourneyMate.Domain.ValueObjects;

public class PlusCode : ValueObject
{
	public string CompoundCode { get; }
	public string GlobalCode { get; }

	public PlusCode(string compoundCode, string globalCode)
	{
		CompoundCode = compoundCode;
		GlobalCode = globalCode;
	}

	protected override IEnumerable<object> GetEqualityComponents()
	{
		yield return CompoundCode;
		yield return GlobalCode;
	}
}