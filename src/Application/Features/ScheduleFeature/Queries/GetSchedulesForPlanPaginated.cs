using AutoMapper;
using FluentValidation;
using JourneyMate.Application.Common.Exceptions;
using JourneyMate.Application.Common.Interfaces;
using JourneyMate.Application.Common.Mappings;
using JourneyMate.Application.Common.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace JourneyMate.Application.Features.ScheduleFeature.Queries;

public record GetSchedulesForPlanPaginated(Guid PlanId, DateTime Date, int PageNumber, int PageSize) : IRequest<PaginatedList<ScheduleDto>>;

internal sealed class GetSchedulesForPlanPaginatedHandler : IRequestHandler<GetSchedulesForPlanPaginated, PaginatedList<ScheduleDto>>
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

	public async Task<PaginatedList<ScheduleDto>> Handle(GetSchedulesForPlanPaginated request, CancellationToken cancellationToken)
	{
		var userId = _currentUserService.UserId ?? throw new UnauthorizedAccessException();
		var user = await _dbContext.Users.AsNoTracking().FirstOrDefaultAsync(x => x.Id == userId) ?? throw new UserNotFoundException(userId);
		var schedules = await _dbContext.Schedules.Include(x => x.Plan)
			.Include(x => x.Place)
			.Where(x => x.PlanId == request.PlanId && x.Plan.UserId == user.Id && x.StartingDate <= request.Date && (x.EndingDate == null || request.Date <= x.EndingDate))
			.OrderBy(x=> x.StartingDate)
			.AsNoTracking().PaginatedListAsync(request.PageNumber, request.PageSize);
		var planScheduleDtos = _mapper.Map<List<ScheduleDto>>(schedules.Items);
		var result = new PaginatedList<ScheduleDto>(planScheduleDtos, schedules.TotalCount, request.PageNumber, request.PageSize);
		
		return result;
	}
}

public class GetSchedulesForPlanPaginatedValidator : AbstractValidator<GetSchedulesForPlanPaginated>
{
	public GetSchedulesForPlanPaginatedValidator()
	{
		RuleFor(x => x.PlanId)
			.NotEmpty();
		RuleFor(x => x.Date)
			.NotEmpty();
		RuleFor(x => x.PageNumber)
			.NotEmpty().GreaterThan(0);
		RuleFor(x => x.PageSize)
			.NotEmpty().GreaterThan(0);
	}
}