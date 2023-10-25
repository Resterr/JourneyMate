using JourneyMate.Application.Common.Mappings;
using JourneyMate.Domain.Entities;
using JourneyMate.Domain.ValueObjects;

namespace JourneyMate.Application.Common.Models;

public class PlaceDto : IMapFrom<Place>
{
	public Guid? Id { get; set; }
	public string ApiPlaceId { get; set; }
	public AddressDto Address { get; set; }
	public string BusinessStatus { get; set; }
	public string Name { get; set; }
	public double Rating { get; set; }
	public int UserRatingsTotal { get; set; }
	public string Vicinity { get; set; }
	public double DistanceFromAddress { get; set; }
	public Location Location { get; set; }
	public PlusCode PlusCode { get; set; }
	public Photo? Photo { get; set; }
	public List<PlaceTypeDto> Types { get; set; }
}