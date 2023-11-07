using AutoMapper;
using FluentValidation;
using JourneyMate.Application.Common.Models;
using JourneyMate.Infrastructure.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace JourneyMate.Application.Features.PlaceFeature.Queries;

public record SearchPlaces(Guid AddressId, List<string> Types) : IRequest<List<PlaceDto>>;

internal sealed class SearchPlacesHandler : IRequestHandler<SearchPlaces, List<PlaceDto>>
{
	private readonly IApplicationDbContext _dbContext;
	private readonly IMapper _mapper;

	public SearchPlacesHandler(IApplicationDbContext dbContext, IMapper mapper)
	{
		_dbContext = dbContext;
		_mapper = mapper;
	}

	public async Task<List<PlaceDto>> Handle(SearchPlaces request, CancellationToken cancellationToken)
	{
		var types = await _dbContext.PlaceTypes.Where(x => request.Types.Contains(x.Name)).ToListAsync(cancellationToken);

		var places = await _dbContext.Places.Include(x => x.Address)
			.Include(x => x.Types)
			.Where(x => x.AddressId == request.AddressId)
			.ToListAsync(cancellationToken);

		places = places.Where(x => types.Any(y => types.Contains(y))).ToList();

		places = places.Where(x => x.UserRatingsTotal > 100 && x.Rating >= 4.0).ToList();
		
		var result = new List<PlaceDto>();

		foreach (var place in places)
		{
			var addressDto = _mapper.Map<AddressDto>(place.Address);
			
			var newPlaceDto = new PlaceDto()
			{
				Id = place.Id,
				ApiPlaceId = place.ApiPlaceId,
				Address = addressDto,
				BusinessStatus = place.BusinessStatus,
				Name = place.Name,
				Rating = place.Rating,
				UserRatingsTotal = place.UserRatingsTotal,
				Vicinity = place.Vicinity,
				DistanceFromAddress = place.DistanceFromAddress,
				Location = place.Location,
				PlusCode = place.PlusCode,
				Types = place.Types.Select(x => x.Name)
					.ToList()
			};
			
			result.Add(newPlaceDto);
		}

		return result;
	}
}

public class SearchPlacesValidator : AbstractValidator<SearchPlaces>
{
	public SearchPlacesValidator()
	{
		RuleFor(x => x.AddressId)
			.NotNull().NotEmpty();
		RuleFor(x => x.Types)
			.NotNull().NotEmpty();
	}
}