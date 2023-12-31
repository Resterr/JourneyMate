namespace JourneyMate.Application.Common.Models;

public class PhotoDto
{
	public Guid Id { get; set; }
	public byte[] Data { get; private set; }
	public int? Height { get; private set; }
	public int? Width { get; private set; }
	public string PhotoReference { get; private set; }
}