using System.Globalization;
using FluentValidation;
using JourneyMate.Application.Common;
using JourneyMate.Application.Common.Exceptions;
using JourneyMate.Application.Common.Interfaces;
using JourneyMate.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace JourneyMate.Application.Features.PlaceFeature.Commands;

public record GenerateReport(Guid AddressId, List<string> Types, double Distance) : IRequest<Guid>;

internal sealed class GenerateReportHandler : IRequestHandler<GenerateReport, Guid>
{
	private readonly IApplicationDbContext _dbContext;
	private readonly ICurrentUserService _currentUserService;
	private readonly IPlacesApiService _placesApiService;

	public GenerateReportHandler(IApplicationDbContext dbContext, ICurrentUserService currentUserService, IPlacesApiService placesApiService)
	{
		_dbContext = dbContext;
		_currentUserService = currentUserService;
		_placesApiService = placesApiService;
	}

	public async Task<Guid> Handle(GenerateReport request, CancellationToken cancellationToken)
	{
		var userId = _currentUserService.UserId ?? throw new UnauthorizedAccessException();
		var user = await _dbContext.Users.FirstOrDefaultAsync(x => x.Id == userId) ?? throw new UserNotFoundException(userId);

		var address = await _dbContext.Addresses.Include(x => x.Places).FirstOrDefaultAsync(x => x.Id == request.AddressId) ?? throw new AddressNotFoundException(request.AddressId);
		var types = await _dbContext.PlaceTypes.Where(x => request.Types.Contains(x.Name)).ToListAsync(cancellationToken);

		var placesCount = address.Places.Count();
		if(placesCount == 0)
		{
			var locationString = string.Join(',', address.Location.Latitude.ToString(CultureInfo.InvariantCulture), address.Location.Longitude.ToString(CultureInfo.InvariantCulture));
			var placeTypes = await _dbContext.PlaceTypes.ToListAsync();
			var places = await _placesApiService.GetPlacesAsync(locationString,  placeTypes.Select(x => x.ApiName).ToList());
			
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
		
		var placeAddresses = await _dbContext.PlaceAddress.Include(x => x.Place)
			.ThenInclude(x => x.Types)
			.Where(x => x.AddressId == address.Id && x.Place.UserRatingsTotal > 10 && x.Place.Rating > 3.0 && x.DistanceFromAddress <= request.Distance)
			.ToListAsync(cancellationToken);

		var placesToReport = placeAddresses.Select(x => x.Place)
			.OrderBy(x => x.Rating)
			.ToList();
		
		placesToReport = placesToReport.Where(x => x.CheckType(types)).ToList();
		
		var newReport = new Report(user, address, placesToReport, types);

		await _dbContext.Reports.AddAsync(newReport);
		await _dbContext.SaveChangesAsync();

		return newReport.Id;
	}
}

public class GenerateReportValidator : AbstractValidator<GenerateReport>
{
	public GenerateReportValidator()
	{
		RuleFor(x => x.AddressId)
			.NotEmpty();
		RuleFor(x => x.Types)
			.NotEmpty();
		RuleFor(x => x.Distance)
			.NotEmpty().GreaterThanOrEqualTo(1);
	}
}