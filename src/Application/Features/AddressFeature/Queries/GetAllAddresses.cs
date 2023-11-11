using AutoMapper;
using FluentValidation;
using JourneyMate.Application.Common.Mappings;
using JourneyMate.Application.Common.Models;
using JourneyMate.Infrastructure.Persistence;
using MediatR;

namespace JourneyMate.Application.Features.AddressFeature.Queries;

public record GetAllAddresses(int PageNumber, int PageSize) : IRequest<PaginatedList<AddressDto>>;

internal sealed class GetAllAddressesHandler : IRequestHandler<GetAllAddresses, PaginatedList<AddressDto>>
{
	private readonly IApplicationDbContext _dbContext;
	private readonly IMapper _mapper;

	public GetAllAddressesHandler(IApplicationDbContext dbContext, IMapper mapper)
	{
		_dbContext = dbContext;
		_mapper = mapper;
	}

	public async Task<PaginatedList<AddressDto>> Handle(GetAllAddresses request, CancellationToken cancellationToken)
	{
		var addresses = await _dbContext.Addresses.OrderBy(x => x.Locality.LongName)
			.PaginatedListAsync(request.PageNumber, request.PageSize);

		var result = _mapper.Map<PaginatedList<AddressDto>>(addresses);

		return result;
	}
}

public class GetAllAddressesValidator : AbstractValidator<GetAllAddresses>
{
	public GetAllAddressesValidator()
	{
		RuleFor(x => x.PageNumber)
			.NotEmpty().GreaterThan(0);
		RuleFor(x => x.PageSize)
			.NotEmpty().GreaterThan(0);
	}
}