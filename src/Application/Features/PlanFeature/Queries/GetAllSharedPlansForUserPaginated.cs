using AutoMapper;
using FluentValidation;
using JourneyMate.Application.Common.Exceptions;
using JourneyMate.Application.Common.Interfaces;
using JourneyMate.Application.Common.Mappings;
using JourneyMate.Application.Common.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace JourneyMate.Application.Features.PlanFeature.Queries;

public record GetAllSharedPlansForUserPaginated(int PageNumber, int PageSize) : IRequest<PaginatedList<PlanDto>>;

internal sealed class GetAllSharedPlansForUserPaginatedHandler : IRequestHandler<GetAllSharedPlansForUserPaginated, PaginatedList<PlanDto>>
{
	private readonly IApplicationDbContext _dbContext;
	private readonly ICurrentUserService _currentUserService;
	private readonly IMapper _mapper;

	public GetAllSharedPlansForUserPaginatedHandler(IApplicationDbContext dbContext, ICurrentUserService currentUserService, IMapper mapper)
	{
		_dbContext = dbContext;
		_currentUserService = currentUserService;
		_mapper = mapper;
	}

	public async Task<PaginatedList<PlanDto>> Handle(GetAllSharedPlansForUserPaginated request, CancellationToken cancellationToken)
	{
		var userId = _currentUserService.UserId ?? throw new UnauthorizedAccessException();
		var user = await _dbContext.Users.FirstOrDefaultAsync(x => x.Id == userId) ?? throw new UserNotFoundException(userId);
		var plans = await _dbContext.Plans.Include(x => x.Shared)
			.ThenInclude(x => x.Follow).Where(x => x.Shared.Any(y => y.Follow.FollowerId == user.Id)).OrderBy(x=> x.Name)
			.PaginatedListAsync(request.PageNumber, request.PageNumber);
		
		var planDtos = _mapper.Map<List<PlanDto>>(plans.Items);

		return new PaginatedList<PlanDto>(planDtos, plans.TotalCount, request.PageNumber, request.PageSize);
	}
}

public class GetAllSharedPlansForUserPaginatedValidator : AbstractValidator<GetAllSharedPlansForUserPaginated>
{
	public GetAllSharedPlansForUserPaginatedValidator()
	{
		RuleFor(x => x.PageNumber)
			.NotEmpty().GreaterThan(0);
		RuleFor(x => x.PageSize)
			.NotEmpty().GreaterThan(0);
	}
}