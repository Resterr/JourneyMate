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
			.Where(x => x.Plans.Any(y => y.PlanId == request.PlanId) && x.Plans.Any(y => y.Plan.UserId == user.Id)).OrderBy(x=> x.Name).PaginatedListAsync(request.PageNumber, request.PageSize);
		var placesDtos = _mapper.Map<List<PlaceDto>>(places.Items);

		return new PaginatedList<PlaceDto>(placesDtos, places.TotalCount, request.PageNumber, request.PageSize);
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