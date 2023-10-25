using JourneyMate.Domain.Common;

namespace JourneyMate.Domain.Entities;

public class PlaceType : BaseEntity
{
	public string Name { get; private set; }
	public List<Place> Places { get; private set; } = new();

	private PlaceType() { }

	public PlaceType(string name)
	{
		Name = name;
	}
}