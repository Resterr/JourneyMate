using AutoMapper;
using FluentValidation;
using JourneyMate.Application.Common.Exceptions;
using JourneyMate.Application.Common.Interfaces;
using JourneyMate.Domain.Entities;
using JourneyMate.Infrastructure.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace JourneyMate.Application.Features.AddressFeature.Commands;

public record AddAddress(string Locality, string AdministrativeArea, string Country) : IRequest<Guid>
{
	public Guid Id { get; init; } = Guid.NewGuid();
}

internal sealed class AddAddressHandler : IRequestHandler<AddAddress, Guid>
{
	private readonly IApplicationDbContext _dbContext;
	private readonly IGeocodeApiService _geocodeApi;
	private readonly IMapper _mapper;

	public AddAddressHandler(IApplicationDbContext dbContext, IGeocodeApiService geocodeApi, IMapper mapper)
	{
		_dbContext = dbContext;
		_geocodeApi = geocodeApi;
		_mapper = mapper;
	}

	public async Task<Guid> Handle(AddAddress request, CancellationToken cancellationToken)
	{
		var components = $"locality:{request.Locality}|administrative_area:{request.AdministrativeArea}|country:{request.Country}";
		var response = await _geocodeApi.GetAddressAsync(components) ?? throw new AddressNotFound();

		if (await _dbContext.Addresses.AnyAsync(x => x.ApiPlaceId == response.PlaceId)) throw new DataAlreadyTakenException(response.PlaceId, "Address");
		
		var address = new Address(response.PlaceId, response.Location, response.Locality, response.AdministrativeAreaLevel2, response.AdministrativeAreaLevel1, response.Country, response.PostalCode);

		await _dbContext.Addresses.AddAsync(address);
		await _dbContext.SaveChangesAsync();

		return address.Id;
	}
}

public class AddAddressValidator : AbstractValidator<AddAddress>
{
	public AddAddressValidator()
	{
		RuleFor(x => x.Locality)
			.NotEmpty();
		RuleFor(x => x.AdministrativeArea)
			.NotEmpty();
		RuleFor(x => x.Country)
			.NotEmpty();
	}
}