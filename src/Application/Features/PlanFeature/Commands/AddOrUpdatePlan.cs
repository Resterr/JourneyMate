using FluentValidation;
using JourneyMate.Application.Common.Exceptions;
using JourneyMate.Application.Common.Interfaces;
using JourneyMate.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace JourneyMate.Application.Features.PlanFeature.Commands;

public record AddOrUpdatePlan(string Name, List<Guid> PlacesId) : IRequest<Guid>;

internal sealed class AddOrUpdateHandler : IRequestHandler<AddOrUpdatePlan, Guid>
{
	private readonly IApplicationDbContext _dbContext;
	private readonly ICurrentUserService _currentUserService;

	public AddOrUpdateHandler(IApplicationDbContext dbContext, ICurrentUserService currentUserService)
	{
		_dbContext = dbContext;
		_currentUserService = currentUserService;
	}

	public async Task<Guid> Handle(AddOrUpdatePlan request, CancellationToken cancellationToken)
	{
		var userId = _currentUserService.UserId ?? throw new UnauthorizedAccessException();
		var user = await _dbContext.Users.SingleOrDefaultAsync(x => x.Id == userId) ?? throw new UserNotFoundException(userId);
		var plan = await _dbContext.Plans.Include(x => x.Places).SingleOrDefaultAsync(x => x.Name == request.Name && x.UserId == user.Id);

		if (plan == null)
		{
			plan = new Plan(user, request.Name);

			var places = new List<Place>();

			foreach (var placeId in request.PlacesId)
			{
				var place = await _dbContext.Places.SingleOrDefaultAsync(x => x.Id == placeId) ?? throw new PlaceNotFoundException(placeId);
				places.Add(place);
			}

			plan.AddPlaces(places);
			await _dbContext.Plans.AddAsync(plan);
			await _dbContext.SaveChangesAsync();
			
			return new Guid();
		}
		else
		{
			var places = new List<Place>();
			var placePlansToadd = request.PlacesId.Where(x => !plan.Places.Any(y => y.Id == x)).ToList();

			foreach (var placeId in placePlansToadd)
			{
				var place = await _dbContext.Places.SingleOrDefaultAsync(x => x.Id == placeId) ?? throw new PlaceNotFoundException(placeId);
				places.Add(place);
			}

			plan.AddPlaces(places);
			_dbContext.Plans.Update(plan);
			await _dbContext.SaveChangesAsync();

			return plan.Id;
		}
	}
}

public class AddOrUpdateValidator : AbstractValidator<AddOrUpdatePlan>
{
	public AddOrUpdateValidator()
	{
		RuleFor(x => x.Name)
			.NotEmpty();
		RuleFor(x => x.PlacesId)
			.NotEmpty();
	}
}