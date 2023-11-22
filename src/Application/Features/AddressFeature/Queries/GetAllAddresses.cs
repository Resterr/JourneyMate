using AutoMapper;
using JourneyMate.Application.Common.Models;
using JourneyMate.Infrastructure.Persistence;
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
		var addresses = await _dbContext.Addresses.OrderBy(x => x.Locality)
			.ToListAsync();

		var result = _mapper.Map<List<AddressDto>>(addresses);

		return result;
	}
}