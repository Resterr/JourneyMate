using FluentValidation;
using JourneyMate.Application.Common.Exceptions;
using JourneyMate.Application.Common.Interfaces;
using JourneyMate.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace JourneyMate.Application.Features.PlanFeature.Commands;

public record AddPlan(string Name, List<Guid> PlacesId) : IRequest<Guid>;

internal sealed class AddPlanHandler : IRequestHandler<AddPlan, Guid>
{
	private readonly IApplicationDbContext _dbContext;
	private readonly ICurrentUserService _currentUserService;

	public AddPlanHandler(IApplicationDbContext dbContext, ICurrentUserService currentUserService)
	{
		_dbContext = dbContext;
		_currentUserService = currentUserService;
	}

	public async Task<Guid> Handle(AddPlan request, CancellationToken cancellationToken)
	{
		var userId = _currentUserService.UserId ?? throw new UnauthorizedAccessException();
		var user = await _dbContext.Users.FirstOrDefaultAsync(x => x.Id == userId) ?? throw new UserNotFoundException(userId);
		var plan = new Plan(user, request.Name);

		var placePlans = new List<PlacePlanRelation>();

		foreach (var placeId in request.PlacesId)
		{
			var place = await _dbContext.Places.SingleOrDefaultAsync(x => x.Id == placeId) ?? throw new PlaceNotFound(placeId);
			placePlans.Add(new PlacePlanRelation(place, plan));
		}

		plan.AddPlaces(placePlans);
		await _dbContext.Plans.AddAsync(plan);
		await _dbContext.SaveChangesAsync();

		return new Guid();
	}
}

public class AddPlanValidator : AbstractValidator<AddPlan>
{
	public AddPlanValidator()
	{
		RuleFor(x => x.Name)
			.NotEmpty();
		RuleFor(x => x.PlacesId)
			.NotEmpty();
	}
}