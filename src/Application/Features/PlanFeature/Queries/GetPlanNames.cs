using AutoMapper;
using JourneyMate.Application.Common.Exceptions;
using JourneyMate.Application.Common.Interfaces;
using JourneyMate.Application.Common.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace JourneyMate.Application.Features.PlanFeature.Queries;

public record GetPlanNames : IRequest<List<PlanNameDto>>;

internal sealed class GetPlanNamesHandler : IRequestHandler<GetPlanNames, List<PlanNameDto>>
{
	private readonly IApplicationDbContext _dbContext;
	private readonly ICurrentUserService _currentUserService;
	private readonly IMapper _mapper;

	public GetPlanNamesHandler(IApplicationDbContext dbContext, ICurrentUserService currentUserService, IMapper mapper)
	{
		_dbContext = dbContext;
		_currentUserService = currentUserService;
		_mapper = mapper;
	}

	public async Task<List<PlanNameDto>> Handle(GetPlanNames request, CancellationToken cancellationToken)
	{
		var userId = _currentUserService.UserId ?? throw new UnauthorizedAccessException();
		var user = await _dbContext.Users.AsNoTracking().FirstOrDefaultAsync(x => x.Id == userId) ?? throw new UserNotFoundException(userId);
		var plans = await _dbContext.Plans.Where(x => x.UserId == user.Id).OrderBy(x=> x.Name).AsNoTracking().ToListAsync();
		var result = _mapper.Map<List<PlanNameDto>>(plans);
 
		return result;
	}
}