using AutoMapper;
using FluentValidation;
using JourneyMate.Application.Common.Models;
using JourneyMate.Infrastructure.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace JourneyMate.Application.Features.AddressFeature.Queries;

public record GetAllTypes : IRequest<List<PlaceTypeDto>>;

internal sealed class GetAllTypesHandler : IRequestHandler<GetAllTypes, List<PlaceTypeDto>>
{
	private readonly IApplicationDbContext _dbContext;
	private readonly IMapper _mapper;

	public GetAllTypesHandler(IApplicationDbContext dbContext, IMapper mapper)
	{
		_dbContext = dbContext;
		_mapper = mapper;
	}

	public async Task<List<PlaceTypeDto>> Handle(GetAllTypes request, CancellationToken cancellationToken)
	{
		var placeTypes = await _dbContext.PlaceTypes.OrderBy(x => x.Name)
			.ToListAsync();

		var result = _mapper.Map<List<PlaceTypeDto>>(placeTypes);

		return result;
	}
}