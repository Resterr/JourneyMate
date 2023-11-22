namespace JourneyMate.Application.Common.Models;

public class ReportDto
{
	public Guid Id { get; set; }
	public Guid AddressId { get; set; }
	public int Rating { get; set; }
	public List<string> Types { get; set; }
}