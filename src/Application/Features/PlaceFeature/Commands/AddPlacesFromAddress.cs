using System.Globalization;
using FluentValidation;
using JourneyMate.Application.Common;
using JourneyMate.Application.Common.Exceptions;
using JourneyMate.Application.Common.Interfaces;
using JourneyMate.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace JourneyMate.Application.Features.PlaceFeature.Commands;

public record AddPlacesFromAddress(Guid AddressId) : IRequest<Unit>;

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
		var placeTypes = await _dbContext.PlaceTypes.ToListAsync();
		var places = await _placesApiService.GetPlacesAsync(locationString,  placeTypes.Select(x => x.ApiName).ToList());
		
		if (places.Count > 0)
		{
			foreach (var placeDto in places)
			{
				var place = await _dbContext.Places.Include(x => x.Addresses).SingleOrDefaultAsync(x => x.ApiPlaceId == placeDto.ApiPlaceId);

				if (place == null)
				{
					if(placeDto.UserRatingsTotal < 10 || placeDto.Rating < 3.0) continue;
					place = new Place(placeDto.ApiPlaceId, placeDto.BusinessStatus, placeDto.Name, placeDto.Rating, placeDto.UserRatingsTotal, placeDto.Vicinity, placeDto.Location, placeDto.PlusCode);

					var distanceFromAddress = Helpers.CalculateDistance(address.Location.Latitude, address.Location.Longitude, place.Location.Latitude, place.Location.Longitude);
					if(distanceFromAddress > 20) continue;
					var placeAddress = new PlaceAddressRelation(address, place, distanceFromAddress);
				
					place.AddAddress(placeAddress);
					
					var typesToSet = placeTypes.Where(placeType => placeDto.Types.Any(x => x.Contains(placeType.ApiName))).ToList();

					place.SetTypes(typesToSet);

					if (placeDto.Photo != null)
					{
						var photoData = await _placesApiService.LoadPhoto(placeDto.Photo.PhotoReference, placeDto.Photo.Height, placeDto.Photo.Width);
						var photo = new Photo(place, placeDto.Photo.Height, placeDto.Photo.Width, placeDto.Photo.PhotoReference, photoData);
						place.AddPhoto(photo);
					}
					
					await _dbContext.Places.AddAsync(place);
				}
				else
				{
					place.UpdateRatings(placeDto.Rating, placeDto.UserRatingsTotal);

					if (!place.Addresses.Any(x => x.AddressId == request.AddressId))
					{
						var distanceFromAddress = Helpers.CalculateDistance(address.Location.Latitude, address.Location.Longitude, place.Location.Latitude, place.Location.Longitude);
						if(distanceFromAddress > 20) continue;
						var placeAddress = new PlaceAddressRelation(address, place, distanceFromAddress);
					
						place.AddAddress(placeAddress);
					}
					
					_dbContext.Places.Update(place);
				}
				
				await _dbContext.SaveChangesAsync();
			}
		}
		else
		{
			throw new PlaceNotFound();
		}

		return Unit.Value;
	}
}

public class AddPlacesFromAddressValidator : AbstractValidator<AddPlacesFromAddress>
{
	public AddPlacesFromAddressValidator()
	{
		RuleFor(x => x.AddressId)
			.NotEmpty();
	}
}