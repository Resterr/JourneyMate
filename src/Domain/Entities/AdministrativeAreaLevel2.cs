using JourneyMate.Domain.Common;

namespace JourneyMate.Domain.Entities;

public class AdministrativeAreaLevel2 : BaseEntity
{
	public string ShortName { get; private set; }
	public string LongName { get; private set; }
	public Guid AdministrativeAreaLevel1Id  { get; set; }
	public AdministrativeAreaLevel1 AdministrativeAreaLevel1 { get; private set; }

	private AdministrativeAreaLevel2() { }

	public AdministrativeAreaLevel2(string shortName, string longName, AdministrativeAreaLevel1 administrativeAreaLevel1)
	{
		ShortName = shortName;
		LongName = longName;
		AdministrativeAreaLevel1 = administrativeAreaLevel1;
	}
}