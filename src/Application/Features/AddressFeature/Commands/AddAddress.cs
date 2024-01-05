using AutoMapper;
using FluentValidation;
using JourneyMate.Application.Common.Exceptions;
using JourneyMate.Application.Common.Interfaces;
using JourneyMate.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace JourneyMate.Application.Features.AddressFeature.Commands;

public record AddAddress(string AdministrativeAreaLevel2, string Locality) : IRequest<Guid>;

internal sealed class AddAddressHandler : IRequestHandler<AddAddress, Guid>
{
	private readonly IApplicationDbContext _dbContext;
	private readonly IGeocodeApiService _geocodeApiService;
	private readonly IMapper _mapper;

	public AddAddressHandler(IApplicationDbContext dbContext, IGeocodeApiService geocodeApiService, IMapper mapper)
	{
		_dbContext = dbContext;
		_geocodeApiService = geocodeApiService;
		_mapper = mapper;
	}

	public async Task<Guid> Handle(AddAddress request, CancellationToken cancellationToken)
	{
		var component = $"locality:{request.Locality}|administrative_area:{request.AdministrativeAreaLevel2}|country:Polska";
		var country = await _dbContext.Countries.SingleOrDefaultAsync(x => x.LongName == "Poland") ?? throw new ObjectNotFoundException("Country");
		var address = await _geocodeApiService.GetAddressAsync(component, request.Locality) ?? throw new AddressNotFoundException();

		var level1 = _dbContext.AdministrativeAreaLevel1.SingleOrDefault(x => x.LongName == address.AdministrativeArea.Level1.LongName);
		if (level1 == null)
		{
			level1 = new AdministrativeAreaLevel1(address.AdministrativeArea.Level1.ShortName, address.AdministrativeArea.Level1.LongName, country);
			_dbContext.AdministrativeAreaLevel1.Add(level1);
			_dbContext.SaveChanges();
		}

		var level2 = _dbContext.AdministrativeAreaLevel2.SingleOrDefault(x => x.LongName == address.AdministrativeArea.Level2.LongName);
		if (level2 == null)
		{
			level2 = new AdministrativeAreaLevel2(address.AdministrativeArea.Level2.ShortName, address.AdministrativeArea.Level2.LongName, level1);
			_dbContext.AdministrativeAreaLevel2.Add(level2);
			_dbContext.SaveChanges();
		}

		var locality = _dbContext.Addresses.SingleOrDefault(x => x.Locality.LongName == address.Locality.LongName);
		if (locality == null)
		{
			locality = new Address(address.ApiPlaceId, address.Locality, address.Location, level2, address.PostalCode);
			_dbContext.Addresses.Add(locality);
		}

		return locality.Id;
	}
}

public class AddAddressValidator : AbstractValidator<AddAddress>
{
	public AddAddressValidator()
	{
		RuleFor(x => x.AdministrativeAreaLevel2)
			.NotEmpty();
		RuleFor(x => x.Locality)
			.NotEmpty();
	}
}