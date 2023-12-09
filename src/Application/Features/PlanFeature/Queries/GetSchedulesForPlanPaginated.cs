using AutoMapper;
using FluentValidation;
using JourneyMate.Application.Common.Exceptions;
using JourneyMate.Application.Common.Interfaces;
using JourneyMate.Application.Common.Mappings;
using JourneyMate.Application.Common.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace JourneyMate.Application.Features.PlanFeature.Queries;

public record GetSchedulesForPlanPaginated(Guid PlanId, int PageNumber, int PageSize) : IRequest<PaginatedList<PlanScheduleDto>>;

internal sealed class GetSchedulesForPlanPaginatedHandler : IRequestHandler<GetSchedulesForPlanPaginated, PaginatedList<PlanScheduleDto>>
{
	private readonly IApplicationDbContext _dbContext;
	private readonly ICurrentUserService _currentUserService;
	private readonly IMapper _mapper;

	public GetSchedulesForPlanPaginatedHandler(IApplicationDbContext dbContext, ICurrentUserService currentUserService, IMapper mapper)
	{
		_dbContext = dbContext;
		_currentUserService = currentUserService;
		_mapper = mapper;
	}

	public async Task<PaginatedList<PlanScheduleDto>> Handle(GetSchedulesForPlanPaginated request, CancellationToken cancellationToken)
	{
		var userId = _currentUserService.UserId ?? throw new UnauthorizedAccessException();
		var user = await _dbContext.Users.FirstOrDefaultAsync(x => x.Id == userId) ?? throw new UserNotFoundException(userId);
		var plans = await _dbContext.PlacePlans.Include(x => x.Plan).Include(x => x.Place)
			.Where(x => x.PlanId == request.PlanId && x.Plan.UserId == user.Id).OrderBy(x=> x.Place.Name).PaginatedListAsync(request.PageNumber, request.PageSize);

		var planScheduleDtos = _mapper.Map<List<PlanScheduleDto>>(plans.Items);

		return new PaginatedList<PlanScheduleDto>(planScheduleDtos, plans.TotalCount, request.PageNumber, request.PageSize);
	}
}

public class GetSchedulesForPlanPaginatedValidator : AbstractValidator<GetSchedulesForPlanPaginated>
{
	public GetSchedulesForPlanPaginatedValidator()
	{
		RuleFor(x => x.PlanId)
			.NotEmpty();
		RuleFor(x => x.PageNumber)
			.NotEmpty().GreaterThan(0);
		RuleFor(x => x.PageSize)
			.NotEmpty().GreaterThan(0);
	}
}