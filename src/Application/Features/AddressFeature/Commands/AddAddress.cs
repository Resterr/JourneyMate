using AutoMapper;
using FluentValidation;
using JourneyMate.Application.Common.Exceptions;
using JourneyMate.Application.Common.Interfaces;
using JourneyMate.Domain.Entities;
using JourneyMate.Domain.Repositories;
using MediatR;

namespace JourneyMate.Application.Features.AddressFeature.Commands;

public record AddAddress(string Locality, string AdministrativeArea, string Country) : IRequest<Guid>
{
	public Guid Id { get; init; } = Guid.NewGuid();
}

internal sealed class AddAddressHandler : IRequestHandler<AddAddress, Guid>
{
	private readonly IAddressRepository _addressRepository;
	private readonly IMapper _mapper;
	private readonly IAvailabilityService _availabilityService;
	private readonly IGeocodeApiService _geocodeApi;

	public AddAddressHandler(IGeocodeApiService geocodeApi, IAvailabilityService availabilityService, IAddressRepository addressRepository, IMapper mapper)
	{
		_geocodeApi = geocodeApi;
		_availabilityService = availabilityService;
		_addressRepository = addressRepository;
		_mapper = mapper;
	}

	public async Task<Guid> Handle(AddAddress request, CancellationToken cancellationToken)
	{
		var components = $"locality:{request.Locality}|administrative_area:{request.AdministrativeArea}|country:{request.Country}";
		var response = await _geocodeApi.GetAddressAsync(components) ?? throw new AddressNotFound();

		var isAvailable = await _availabilityService.CheckAddress(response.PlaceId);
		if (isAvailable)
		{
			var address = new Address(response.PlaceId, response.Location, response.Locality, response.AdministrativeAreaLevel2, response.AdministrativeAreaLevel1, response.Country,
				response.PostalCode);

			await _addressRepository.AddAsync(address);
			
			return address.Id;
		}
		else
		{
			throw new AddressAlreadyAdded();
		}
		
	}
}

public class AddAddressValidator : AbstractValidator<AddAddress>
{
	public AddAddressValidator()
	{
		RuleFor(x => x.Locality)
			.NotNull();
		RuleFor(x => x.AdministrativeArea)
			.NotNull();
		RuleFor(x => x.Country)
			.NotNull();
	}
}