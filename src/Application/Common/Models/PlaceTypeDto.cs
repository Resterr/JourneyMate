using JourneyMate.Application.Common.Mappings;
using JourneyMate.Domain.Entities;

namespace JourneyMate.Application.Common.Models;

public class PlaceTypeDto : IMapFrom<PlaceType>
{
	public Guid? Id { get; set; }
	public string Name { get; set; }
}