using JourneyMate.Domain.Common;

namespace JourneyMate.Domain.ValueObjects;

public class Photo : ValueObject
{
	public int? Height { get; set; }
	public int? Width { get; set; }
	public string PhotoReference { get; set; }

	public Photo(int? height, int? width, string photoReference)
	{
		Height = height;
		Width = width;
		PhotoReference = photoReference;
	}

	protected override IEnumerable<object> GetEqualityComponents()
	{
		if (Height != null) yield return Height;
		if (Width != null) yield return Width;
		yield return PhotoReference;
	}
}