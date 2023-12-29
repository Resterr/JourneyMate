using JourneyMate.Application.Common.Mappings;
using JourneyMate.Domain.Entities;

namespace JourneyMate.Application.Common.Models;

public class PlanDto : IMapFrom<Plan>
{
	public Guid Id { get; set; }
	public string Name { get; set; }
	public DateTime Created { get; set; }
}