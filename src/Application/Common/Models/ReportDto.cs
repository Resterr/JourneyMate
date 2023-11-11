namespace JourneyMate.Application.Common.Models;

public class ReportDto
{
	public Guid Id { get; set; }
	public int Rating { get; set; }
	public List<PlaceDto> Places { get; set; }
}