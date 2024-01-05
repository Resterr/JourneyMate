using AutoMapper;
using JourneyMate.Application.Common.Interfaces;
using JourneyMate.Application.Common.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace JourneyMate.Application.Features.AddressFeature.Queries;

public record GetAllAddresses : IRequest<List<AddressDto>>;

internal sealed class GetAllAddressesHandler : IRequestHandler<GetAllAddresses, List<AddressDto>>
{
	private readonly IApplicationDbContext _dbContext;
	private readonly IMapper _mapper;

	public GetAllAddressesHandler(IApplicationDbContext dbContext, IMapper mapper)
	{
		_dbContext = dbContext;
		_mapper = mapper;
	}

	public async Task<List<AddressDto>> Handle(GetAllAddresses request, CancellationToken cancellationToken)
	{
		var addresses = await _dbContext.Addresses.Include(x => x.AdministrativeAreaLevel2)
			.ThenInclude(x => x.AdministrativeAreaLevel1)
			.ThenInclude(x => x.Country)
			.OrderBy(x => x.Locality.LongName)
			.AsNoTracking()
			.ToListAsync();

		var result = _mapper.Map<List<AddressDto>>(addresses);

		return result;
	}
}