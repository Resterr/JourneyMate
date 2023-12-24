using AutoMapper;
using JourneyMate.Application.Common.Models;
using JourneyMate.Domain.Entities;

namespace JourneyMate.Application.Common.Mappings;

public interface IMapFrom<T>
{
	void Mapping(Profile profile)
	{
		profile.CreateMap<PlacePlanRelation, PlanScheduleDto>()
			.ForMember(dest => dest.PlanId, opt => opt.MapFrom(src => src.Plan.Id))
			.ForMember(dest => dest.PlanName, opt => opt.MapFrom(src => src.Plan.Name))
			.ForMember(dest => dest.PlaceId, opt => opt.MapFrom(src => src.Place.Id))
			.ForMember(dest => dest.PlaceName, opt => opt.MapFrom(src => src.Place.Name));
		profile.CreateMap(typeof(T), GetType());
	}
}