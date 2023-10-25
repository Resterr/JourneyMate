using System.Globalization;
using FluentValidation;
using JourneyMate.Application.Common.Interfaces;
using JourneyMate.Domain.Entities;
using JourneyMate.Domain.Repositories;
using MediatR;

namespace JourneyMate.Application.Features.PlaceFeature.Commands;

public record AddPlacesFromAddress(Guid AddressId, string Type) : IRequest<Unit>;

internal sealed class AddPlacesFromAddressHandler : IRequestHandler<AddPlacesFromAddress, Unit>
{
	private readonly IAddressRepository _addressRepository;
	private readonly IPlaceRepository _placeRepository;
	private readonly IPlacesApiService _placesApiService;
	private readonly IPlaceTypeRepository _placeTypeRepository;


	public AddPlacesFromAddressHandler(IAddressRepository addressRepository, IPlaceRepository placeRepository, IPlaceTypeRepository placeTypeRepository, IPlacesApiService placesApiService)
	{
		_addressRepository = addressRepository;
		_placeRepository = placeRepository;
		_placeTypeRepository = placeTypeRepository;
		_placesApiService = placesApiService;
	}

	public async Task<Unit> Handle(AddPlacesFromAddress request, CancellationToken cancellationToken)
	{
		var address = await _addressRepository.GetByIdAsync(request.AddressId);
		//var locationString = address.Location.Latitude + "," + address.Location.Longitude.ToString();
		var locationString = string.Join(',', address.Location.Latitude.ToString(CultureInfo.InvariantCulture), address.Location.Longitude.ToString(CultureInfo.InvariantCulture));
		var response = await _placesApiService.GetPlacesAsync(locationString, "50000", request.Type, "prominence", address.Location.Latitude, address.Location.Longitude);

		if (response.Count > 0)
		{
			var types = response.SelectMany(x => x.Types)
				.DistinctBy(x => x.Name)
				.Select(x => new PlaceType(x.Name))
				.ToList();
			await _placeTypeRepository.AddRangeAsync(types);

			var placeTypes = await _placeTypeRepository.GetAllAsync();

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

			await _placeRepository.AddRangeAsync(places);
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