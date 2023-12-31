using JourneyMate.Application.Common.Mappings;
using JourneyMate.Domain.Entities;

namespace JourneyMate.Application.Common.Models;

public class PlanNameDto : IMapFrom<Plan>
{
	public Guid Id { get; set; }
	public string Name { get; set; }
}