using JourneyMate.Application.Common.Mappings;
using JourneyMate.Domain.Entities;

namespace JourneyMate.Application.Common.Models;

public class PlaceNameDto : IMapFrom<Place>
{
	public Guid Id { get; set; }
	public string Name { get; set; }
}