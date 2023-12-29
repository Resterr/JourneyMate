using AutoMapper;
using FluentValidation;
using JourneyMate.Application.Common.Exceptions;
using JourneyMate.Application.Common.Interfaces;
using JourneyMate.Application.Common.Mappings;
using JourneyMate.Application.Common.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace JourneyMate.Application.Features.PlanFeature.Queries;

public record GetSharedPlacesForPlanPaginated(Guid PlanId, int PageNumber, int PageSize, string? TagsString) : IRequest<PaginatedList<PlaceDto>>;

internal sealed class GetSharedPlacesForPlanPaginatedHandler : IRequestHandler<GetSharedPlacesForPlanPaginated, PaginatedList<PlaceDto>>
{
	private readonly ICurrentUserService _currentUserService;
	private readonly IApplicationDbContext _dbContext;
	private readonly IMapper _mapper;

	public GetSharedPlacesForPlanPaginatedHandler(IApplicationDbContext dbContext, ICurrentUserService currentUserService, IMapper mapper)
	{
		_dbContext = dbContext;
		_currentUserService = currentUserService;
		_mapper = mapper;
	}

	public async Task<PaginatedList<PlaceDto>> Handle(GetSharedPlacesForPlanPaginated request, CancellationToken cancellationToken)
	{
		var userId = _currentUserService.UserId ?? throw new UnauthorizedAccessException();
		var user = await _dbContext.Users.FirstOrDefaultAsync(x => x.Id == userId) ?? throw new UserNotFoundException(userId);
		if (request.TagsString != null)
		{
			if (await _dbContext.Plans.AnyAsync(x => x.Id == request.PlanId) == false) throw new PlanNotFoundException(request.PlanId);

			var typesNames = request.TagsString.Split('|');
			var types = await _dbContext.PlaceTypes.Where(x => typesNames.Contains(x.Name))
				.ToListAsync(cancellationToken);

			var places = await _dbContext.Places.Include(x => x.Plans)
				.ThenInclude(x => x.Plan)
				.ThenInclude(x => x.Shared)
				.ThenInclude(x => x.Follow)
				.Include(x => x.Addresses)
				.Include(x => x.Types)
				.Where(x => x.Plans.Any(y => y.PlanId == request.PlanId)
					&& x.Plans.Any(y => y.Plan.Shared.Any(z => z.Follow.FollowerId == user.Id) 
						&& x.Types.Any(z => types.Contains(z))))
				.OrderBy(x => x.Rating)
				.PaginatedListAsync(request.PageNumber, request.PageSize);
			
			var placesDto = _mapper.Map<List<PlaceDto>>(places.Items);
			placesDto.ForEach(placeDto => placeDto.DistanceFromAddress = Math.Round(placeDto.DistanceFromAddress, 2));
			var result = new PaginatedList<PlaceDto>(placesDto, places.TotalCount, request.PageNumber, request.PageSize);

			return result;
		}
		else
		{
			if (await _dbContext.Plans.AnyAsync(x => x.Id == request.PlanId) == false) throw new PlanNotFoundException(request.PlanId);

			var places = await _dbContext.Places.Include(x => x.Plans)
				.ThenInclude(x => x.Plan)
				.ThenInclude(x => x.Shared)
				.ThenInclude(x => x.Follow)
				.Include(x => x.Addresses)
				.Include(x => x.Types)
				.Where(x => x.Plans.Any(y => y.PlanId == request.PlanId) && x.Plans.Any(y => y.Plan.Shared.Any(z => z.Follow.FollowerId == user.Id)))
				.OrderBy(x => x.Rating)
				.PaginatedListAsync(request.PageNumber, request.PageSize);
			
			var placesDto = _mapper.Map<List<PlaceDto>>(places.Items);
			placesDto.ForEach(placeDto => placeDto.DistanceFromAddress = Math.Round(placeDto.DistanceFromAddress, 2));
			var result = new PaginatedList<PlaceDto>(placesDto, places.TotalCount, request.PageNumber, request.PageSize);

			return result;
		}
	}
}

public class GetSharedPlacesForPlanPaginatedValidator : AbstractValidator<GetSharedPlacesForPlanPaginated>
{
	public GetSharedPlacesForPlanPaginatedValidator()
	{
		RuleFor(x => x.PlanId)
			.NotEmpty();
		RuleFor(x => x.PageNumber)
			.NotEmpty()
			.GreaterThan(0);
		RuleFor(x => x.PageSize)
			.NotEmpty()
			.GreaterThan(0);
	}
}