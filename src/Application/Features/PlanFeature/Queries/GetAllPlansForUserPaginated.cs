using AutoMapper;
using FluentValidation;
using JourneyMate.Application.Common.Exceptions;
using JourneyMate.Application.Common.Interfaces;
using JourneyMate.Application.Common.Mappings;
using JourneyMate.Application.Common.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace JourneyMate.Application.Features.PlanFeature.Queries;

public record GetAllPlansForUserPaginated(int PageNumber, int PageSize) : IRequest<PaginatedList<PlanDto>>;

internal sealed class GetAllPlansForUserPaginatedHandler : IRequestHandler<GetAllPlansForUserPaginated, PaginatedList<PlanDto>>
{
	private readonly IApplicationDbContext _dbContext;
	private readonly ICurrentUserService _currentUserService;
	private readonly IMapper _mapper;

	public GetAllPlansForUserPaginatedHandler(IApplicationDbContext dbContext, ICurrentUserService currentUserService, IMapper mapper)
	{
		_dbContext = dbContext;
		_currentUserService = currentUserService;
		_mapper = mapper;
	}

	public async Task<PaginatedList<PlanDto>> Handle(GetAllPlansForUserPaginated request, CancellationToken cancellationToken)
	{
		var userId = _currentUserService.UserId ?? throw new UnauthorizedAccessException();
		var user = await _dbContext.Users.FirstOrDefaultAsync(x => x.Id == userId) ?? throw new UserNotFoundException(userId);
		var plans = await _dbContext.Plans.Where(x => x.UserId == user.Id).OrderBy(x=> x.Name).PaginatedListAsync(request.PageNumber, request.PageSize);
		var planDtos = _mapper.Map<List<PlanDto>>(plans.Items);

		return new PaginatedList<PlanDto>(planDtos, plans.TotalCount, request.PageNumber, request.PageSize);
	}
}

public class GetAllPlansForUserPaginatedValidator : AbstractValidator<GetAllPlansForUserPaginated>
{
	public GetAllPlansForUserPaginatedValidator()
	{
		RuleFor(x => x.PageNumber)
			.NotEmpty().GreaterThan(0);
		RuleFor(x => x.PageSize)
			.NotEmpty().GreaterThan(0);
	}
}