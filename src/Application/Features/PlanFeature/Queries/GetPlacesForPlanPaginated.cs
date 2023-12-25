using AutoMapper;
using FluentValidation;
using JourneyMate.Application.Common.Exceptions;
using JourneyMate.Application.Common.Interfaces;
using JourneyMate.Application.Common.Mappings;
using JourneyMate.Application.Common.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace JourneyMate.Application.Features.PlanFeature.Queries;

public record GetPlacesForPlanPaginated(Guid PlanId, int PageNumber, int PageSize) : IRequest<PaginatedList<PlaceDto>>;

internal sealed class GetPlacesForPlanPaginatedHandler : IRequestHandler<GetPlacesForPlanPaginated, PaginatedList<PlaceDto>>
{
	private readonly IApplicationDbContext _dbContext;
	private readonly ICurrentUserService _currentUserService;
	private readonly IMapper _mapper;

	public GetPlacesForPlanPaginatedHandler(IApplicationDbContext dbContext, ICurrentUserService currentUserService, IMapper mapper)
	{
		_dbContext = dbContext;
		_currentUserService = currentUserService;
		_mapper = mapper;
	}

	public async Task<PaginatedList<PlaceDto>> Handle(GetPlacesForPlanPaginated request, CancellationToken cancellationToken)
	{
		var userId = _currentUserService.UserId ?? throw new UnauthorizedAccessException();
		var user = await _dbContext.Users.FirstOrDefaultAsync(x => x.Id == userId) ?? throw new UserNotFoundException(userId);
		var places = await _dbContext.Places.Include(x => x.Plans).ThenInclude(x => x.Plan)
			.Include(x => x.Addresses)
			.Where(x => x.Plans.Any(y => y.PlanId == request.PlanId) && x.Plans.Any(y => y.Plan.UserId == user.Id))
			.OrderBy(x=> x.Rating)
			.PaginatedListAsync(request.PageNumber, request.PageSize);
		
		var placesDto = _mapper.Map<List<PlaceDto>>(places.Items);
		placesDto.ForEach(placeDto => placeDto.DistanceFromAddress = Math.Round(placeDto.DistanceFromAddress, 2));
		var result = new PaginatedList<PlaceDto>(placesDto, places.TotalCount, request.PageNumber, request.PageSize);

		return result;
	}
}

public class GetPlacesForPlanPaginatedHandlerValidator : AbstractValidator<GetPlacesForPlanPaginated>
{
	public GetPlacesForPlanPaginatedHandlerValidator()
	{
		RuleFor(x => x.PlanId)
			.NotEmpty();
		RuleFor(x => x.PageNumber)
			.NotEmpty().GreaterThan(0);
		RuleFor(x => x.PageSize)
			.NotEmpty().GreaterThan(0);
	}
}