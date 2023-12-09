﻿using System.Globalization;
using FluentValidation;
using JourneyMate.Application.Common.Exceptions;
using JourneyMate.Application.Common.Interfaces;
using JourneyMate.Domain.Entities;
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
			//Add missing types
			var types = response.SelectMany(x => x.Types)
				.DistinctBy(x => x.Name)
				.Select(x => new PlaceType(x.Name))
				.ToList();
			
			var existingPlaceTypes = _dbContext.PlaceTypes.ToList();
			foreach (var placeType in types)
			{
				if (!existingPlaceTypes.Any(x => x.Name == placeType.Name))
				{
					_dbContext.PlaceTypes.Add(placeType);
				}
			}
			_dbContext.SaveChanges();

			var placeTypes = await _dbContext.PlaceTypes.ToListAsync();
			var places = response.Select(placeDto =>
			{
				var place = new Place(placeDto.ApiPlaceId, placeDto.BusinessStatus, placeDto.Name, placeDto.Rating, placeDto.UserRatingsTotal, placeDto.Vicinity, placeDto.Location,
					placeDto.PlusCode, placeDto.Photo);

				var placeAddress = new PlaceAddress(address, place, placeDto.DistanceFromAddress);
				
				place.AddAddress(placeAddress);
				
				var typesToSet = placeTypes.Where(placeType => placeDto.Types.Any(x => x.Name.Contains(placeType.Name)))
					.ToList();
				
				place.SetTypes(typesToSet);

				return place;
			}).ToList();

			places = places.Where(x => x.UserRatingsTotal > 50 && x.Rating >= 3.0).ToList();
			
			foreach (var place in places)
			{
				var existingPlace = await _dbContext.Places.Include(x => x.Addresses).FirstOrDefaultAsync(x => x.ApiPlaceId == place.ApiPlaceId);
				if (existingPlace == null)
				{
					if (place.Photo != null)
					{
						if (place.Photo.Height is null || place.Photo.Width is null)
						{
							place.Photo.LoadPhotoData(await _placesApiService.LoadPhoto(place.Photo.PhotoReference, 500, 500));
						}
						else
						{
							place.Photo.LoadPhotoData(await _placesApiService.LoadPhoto(place.Photo.PhotoReference, (int)place.Photo.Height, (int)place.Photo.Width));
						}
						
					}
					await _dbContext.Places.AddAsync(place);
				}
				else
				{
					existingPlace.UpdateRatings(place.Rating, place.UserRatingsTotal);
					if (!existingPlace.CheckAddress(address.Id))
					{
						existingPlace.AddAddress(place.Addresses[0]);
					}
					
					_dbContext.Places.Update(existingPlace);
				}
			}
			
			await _dbContext.SaveChangesAsync();
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
		RuleFor(x => x.Type)
			.NotEmpty();
	}
}