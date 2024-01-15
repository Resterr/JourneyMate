using JourneyMate.Domain.Common;

namespace JourneyMate.Domain.Entities;

public class PlaceType : BaseEntity
{
	public string ApiName { get; private set; }
	public string Name { get; private set; }
	public List<Place> Places { get; private set; } = new();
	public List<Report> Reports { get; private set; } = new();

	private PlaceType() { }

	public PlaceType(string apiName, string name)
	{
		ApiName = apiName;
		Name = name;
	}
}