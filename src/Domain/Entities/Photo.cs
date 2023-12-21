using JourneyMate.Domain.Common;

namespace JourneyMate.Domain.Entities;

public class Photo : BaseEntity
{
	public Guid PlaceId { get; private set; }
	public Place Place { get; private set; }
	public byte[] Data { get; private set; }
	public int? Height { get; private set; }
	public int? Width { get; private set; }
	public string PhotoReference { get; private set; }
	
	private Photo() { }
	public Photo(Place place, int? height, int? width, string photoReference, byte[] data)
	{
		Place = place;
		Height = height;
		Width = width;
		PhotoReference = photoReference;
	}

	public void UpdatePhoto(string? photoReference, int? height, int? width, byte[] data)
	{
		if (photoReference != null) PhotoReference = photoReference;
		if (height != null) Height = height;
		if (width != null) Width = width;
		Data = data;
	}
}