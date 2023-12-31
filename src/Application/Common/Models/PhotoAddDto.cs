namespace JourneyMate.Application.Common.Models;

public class PhotoAddDto
{
	public int Height { get; private set; }
	public int Width { get; private set; }
	public string PhotoReference { get; private set; }
	
	public PhotoAddDto(int height, int width, string photoReference)
	{
		Height = height;
		Width = width;
		PhotoReference = photoReference;
	}
}