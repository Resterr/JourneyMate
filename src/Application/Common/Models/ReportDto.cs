using JourneyMate.Application.Common.Mappings;
using JourneyMate.Domain.Entities;

namespace JourneyMate.Application.Common.Models;

public class ReportDto : IMapFrom<Report>
{
	public Guid Id { get; set; }
	public Guid AddressId { get; set; }
	public int Rating { get; set; }
	public DateTime Created { get; set; }
	public List<string> Types { get; set; }
}