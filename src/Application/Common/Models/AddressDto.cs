using JourneyMate.Application.Common.Mappings;
using JourneyMate.Domain.Entities;
using JourneyMate.Domain.ValueObjects;

namespace JourneyMate.Application.Common.Models;

public class AddressDto : IMapFrom<Address>
{
	public Guid Id { get; set; }
	public string PlaceId { get; set; }
	public LocationDto Location { get; set; }
	public AddressComponentDto Locality { get; set; }
	public AddressComponentDto AdministrativeAreaLevel2 { get; set; }
	public AddressComponentDto AdministrativeAreaLevel1 { get; set; }
	public AddressComponentDto Country { get; set; }
	public AddressComponentDto PostalCode { get; set; }

	public class AddressComponentDto : IMapFrom<AddressComponent>
	{
		public string ShortName { get; set; }
		public string LongName { get; set; }
	}

	public class LocationDto : IMapFrom<Location>
	{
		public double Latitude { get; set; }
		public double Longitude { get; set; }
	}
}