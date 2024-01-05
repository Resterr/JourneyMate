namespace JourneyMate.Application.Common.Models;

public class PhotoDto
{
	public Guid Id { get; set; }
	public byte[] Data { get; }
	public int? Height { get; }
	public int? Width { get; }
	public string PhotoReference { get; }
}