using JourneyMate.Application.Common.Mappings;
using JourneyMate.Domain.Entities;
using JourneyMate.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace JourneyMate.Application.Common.Models;

public class PlaceDto : IMapFrom<Place>
{
	public Guid Id { get; set; }
	public string ApiPlaceId { get; set; }
	public string BusinessStatus { get;  set; }
	public string Name { get; set; }
	public double Rating { get; set; }
	public int UserRatingsTotal { get; set; }
	public string Vicinity { get; set; }
	public Location Location { get; set; }
	public PlusCode PlusCode { get; set; }
	public Stream? Photo { get; set; }
	public List<PlaceTypeDto> Types { get; set; } = new();

	public void UpdateFromPlace(Place place)
	{
		ApiPlaceId = place.ApiPlaceId;
		BusinessStatus = place.BusinessStatus;
		Name = place.Name;
		Rating = place.Rating;
		UserRatingsTotal = place.UserRatingsTotal;
		Vicinity = place.Vicinity;
		Location = place.Location;
		PlusCode = place.PlusCode;
	}
}