using System.Globalization;
using FluentValidation;
using JourneyMate.Application.Common.Exceptions;
using JourneyMate.Application.Common.Interfaces;
using JourneyMate.Domain.Entities;
using JourneyMate.Infrastructure.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace JourneyMate.Application.Features.PlaceFeature.Commands;

public record AddPlacesFromAddress(Guid AddressId, string Type) : IRequest<Unit>;

internal sealed class AddPlacesFromAddressHandler : IRequestHandler<AddPlacesFromAddress, Unit>
{
	private readonly IApplicationDbContext _dbContext;
	private readonly IPlacesApiService _placesApiService;

	public AddPlacesFromAddressHandler(IApplicationDbContext dbContext, IPlacesApiService placesApiService)
	{
		_dbContext = dbContext;
		_placesApiService = placesApiService;
	}

	public async Task<Unit> Handle(AddPlacesFromAddress request, CancellationToken cancellationToken)
	{
		var address = await _dbContext.Addresses.SingleOrDefaultAsync(x => x.Id == request.AddressId) ?? throw new AddressNotFound(request.AddressId);
		var locationString = string.Join(',', address.Location.Latitude.ToString(CultureInfo.InvariantCulture), address.Location.Longitude.ToString(CultureInfo.InvariantCulture));
		var response = await _placesApiService.GetPlacesAsync(locationString, "50000", request.Type, "prominence", address.Location.Latitude, address.Location.Longitude);

		if (response.Count > 0)
		{
			var types = response.SelectMany(x => x.Types)
				.DistinctBy(x => x.Name)
				.Select(x => new PlaceType(x.Name))
				.ToList();
			
			foreach (var placeType in types)
				if (await _dbContext.PlaceTypes.AnyAsync(x => x.Name == placeType.Name) == false)
					await _dbContext.PlaceTypes.AddAsync(placeType);
			await _dbContext.SaveChangesAsync();

			var placeTypes = await _dbContext.PlaceTypes.ToListAsync();

			var places = new List<Place>();
			foreach (var placeDto in response)
			{
				var place = new Place(placeDto.ApiPlaceId, placeDto.BusinessStatus, placeDto.Name, placeDto.Rating, placeDto.UserRatingsTotal, placeDto.Vicinity, placeDto.DistanceFromAddress, placeDto.Location,
					placeDto.PlusCode, placeDto.Photo, address.Id);

				var currentPlaceTypes = placeTypes.Where(placeType => placeDto.Types.Any(x => x.Name.Contains(placeType.Name)))
					.ToList();

				place.SetTypes(currentPlaceTypes);
				places.Add(place);
			}

			foreach (var placeType in placeTypes)
				if (await _dbContext.PlaceTypes.AnyAsync(x => x.Name == placeType.Name) == false)
					await _dbContext.PlaceTypes.AddAsync(placeType);
			
			await _dbContext.SaveChangesAsync();
		}

		return Unit.Value;
	}
}

public class AddPlacesFromAddressValidator : AbstractValidator<AddPlacesFromAddress>
{
	public AddPlacesFromAddressValidator()
	{
		RuleFor(x => x.AddressId)
			.NotNull();
		RuleFor(x => x.Type)
			.NotNull();
	}
}