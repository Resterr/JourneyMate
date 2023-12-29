using AutoMapper;
using JourneyMate.Application.Common.Models;
using JourneyMate.Domain.Entities;

namespace JourneyMate.Application.Common.Mappings;

public interface IMapFrom<T>
{
	void Mapping(Profile profile)
	{
		profile.CreateMap<Address, AddressDto>()
			.ForMember(dest => dest.Locality, opt => opt.MapFrom(src => src.Locality.LongName))
			.ForMember(dest => dest.AdministrativeAreaLevel2, opt => opt.MapFrom(src => src.AdministrativeAreaLevel2.LongName))
			.ForMember(dest => dest.AdministrativeAreaLevel1, opt => opt.MapFrom(src => src.AdministrativeAreaLevel2.AdministrativeAreaLevel1.LongName))
			.ForMember(dest => dest.Country, opt => opt.MapFrom(src => src.AdministrativeAreaLevel2.AdministrativeAreaLevel1.Country.LongName));
		
		profile.CreateMap<PlacePlanRelation, PlanScheduleDto>()
			.ForMember(dest => dest.PlanId, opt => opt.MapFrom(src => src.Plan.Id))
			.ForMember(dest => dest.PlanName, opt => opt.MapFrom(src => src.Plan.Name))
			.ForMember(dest => dest.PlaceId, opt => opt.MapFrom(src => src.Place.Id))
			.ForMember(dest => dest.PlaceName, opt => opt.MapFrom(src => src.Place.Name));

		profile.CreateMap<Place, PlaceDto>()
			.ForMember(dest => dest.DistanceFromAddress, opt => opt.MapFrom(src => src.Addresses[0].DistanceFromAddress));
		profile.CreateMap(typeof(T), GetType());
	}
}