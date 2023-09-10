using JourneyMate.Application.Common.Exceptions;
using JourneyMate.Application.Common.Interfaces;
using JourneyMate.Application.Common.Security;
using JourneyMate.Domain.Entities;
using JourneyMate.Domain.Repositories;
using JourneyMate.Domain.ValueObjects;
using MediatR;

namespace JourneyMate.Application.Features.AddressFeature.Commands.AddAddress;
[Authorize(Role = "Admin")]
public record AddAddress(string Locality, string AdministrativeArea, string Country) : IRequest<Guid>;

internal sealed class AddAddressHandler : IRequestHandler<AddAddress, Guid>
{
	private readonly IGeocodeApiService _geocodeApi;
	private readonly IAvailabilityService _availabilityService;
	private readonly IAddressRepository _addressRepository;

	public AddAddressHandler(IGeocodeApiService geocodeApi, IAvailabilityService availabilityService, IAddressRepository addressRepository)
	{
		_geocodeApi = geocodeApi;
		_availabilityService = availabilityService;
		_addressRepository = addressRepository;
	}

	public async Task<Guid> Handle(AddAddress request, CancellationToken cancellationToken)
	{
		var components = $"locality:{request.Locality}|administrative_area:{request.AdministrativeArea}|country:{request.Country}";
		var response = await _geocodeApi.GetAddressAsync(components) ?? throw new BadRequestException("Null response.");

		if (response.Status != "OK") {
			throw new BadRequestException(response.Status);
		}

		var result = response.Results[0];

		var available = await _availabilityService.CheckAddress(result.Place_Id);
		var lat = result.Geometry.Location.Lat;

		if (lat == 0.0) throw new BadRequestException();

		var lng = result.Geometry.Location.Lng;

		if (lng == 0.0) throw new BadRequestException();

		var locality = result.Address_Components.FirstOrDefault(x => x.Types.Contains("locality")) ?? throw new BadRequestException();
		var administrativeAreaLevel2 = result.Address_Components.FirstOrDefault(x => x.Types.Contains("administrative_area_level_2")) ?? throw new BadRequestException();
		var administrativeAreaLevel1 = result.Address_Components.FirstOrDefault(x => x.Types.Contains("administrative_area_level_1")) ?? throw new BadRequestException();
		var country = result.Address_Components.FirstOrDefault(x => x.Types.Contains("country")) ?? throw new BadRequestException();
		var postalCode = result.Address_Components.FirstOrDefault(x => x.Types.Contains("postal_code")) ?? throw new BadRequestException();

		var address = new Address(result.Place_Id, new Location(lat, lng), 
			new AddressComponent(locality.Short_Name, locality.Long_Name),
			new AddressComponent(administrativeAreaLevel2.Short_Name, administrativeAreaLevel2.Long_Name),
			new AddressComponent(administrativeAreaLevel1.Short_Name, administrativeAreaLevel1.Long_Name),
			new AddressComponent(country.Short_Name, country.Long_Name),
			new AddressComponent(postalCode.Short_Name, postalCode.Long_Name));

		if (available)
		{
			await _addressRepository.AddAsync(address);
		}

		return address.Id;
	}
}
