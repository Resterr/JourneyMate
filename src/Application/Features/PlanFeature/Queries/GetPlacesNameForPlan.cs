using AutoMapper;
using FluentValidation;
using JourneyMate.Application.Common.Exceptions;
using JourneyMate.Application.Common.Interfaces;
using JourneyMate.Application.Common.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace JourneyMate.Application.Features.PlanFeature.Queries;

public record GetPlacesNameForPlan(Guid PlanId) : IRequest<List<PlaceNameDto>>;

internal sealed class GetPlacesNameForPlanHandler : IRequestHandler<GetPlacesNameForPlan, List<PlaceNameDto>>
{
	private readonly IApplicationDbContext _dbContext;
	private readonly ICurrentUserService _currentUserService;
	private readonly IMapper _mapper;

	public GetPlacesNameForPlanHandler(IApplicationDbContext dbContext, ICurrentUserService currentUserService, IMapper mapper)
	{
		_dbContext = dbContext;
		_currentUserService = currentUserService;
		_mapper = mapper;
	}

	public async Task<List<PlaceNameDto>> Handle(GetPlacesNameForPlan request, CancellationToken cancellationToken)
	{
		var userId = _currentUserService.UserId ?? throw new UnauthorizedAccessException();
		var user = await _dbContext.Users.FirstOrDefaultAsync(x => x.Id == userId) ?? throw new UserNotFoundException(userId);
		var plan = await _dbContext.Plans.Include(x => x.Places)
			.Where(x => x.UserId == user.Id)
			.SingleOrDefaultAsync(x => x.Id == request.PlanId) ?? throw new PlanNotFoundException(request.PlanId);
		var result = _mapper.Map<List<PlaceNameDto>>(plan.Places);
 
		return result;
	}
}

public class GetPlacesNameForPlanValidator : AbstractValidator<GetPlacesNameForPlan>
{
	public GetPlacesNameForPlanValidator()
	{
		RuleFor(x => x.PlanId)
			.NotEmpty();
	}
}