namespace JourneyMate.Application.Common.Models;

public class ReportListDto
{
	public Guid Id { get; set; }
	public Guid AddressId { get; set; }
	public int Rating { get; set; }
	public List<Guid> Places { get; set; }
	public List<string> Types { get; set; }
}