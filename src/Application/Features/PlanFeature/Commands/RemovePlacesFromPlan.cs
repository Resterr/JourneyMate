using FluentValidation;
using JourneyMate.Application.Common.Exceptions;
using JourneyMate.Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace JourneyMate.Application.Features.PlanFeature.Commands;

public record RemovePlacesFromPlan(Guid PlanId, List<Guid> Places) : IRequest<Unit>;

internal sealed class RemovePlacesFromPlanPlanHandler : IRequestHandler<RemovePlacesFromPlan, Unit>
{
	private readonly IApplicationDbContext _dbContext;

	public RemovePlacesFromPlanPlanHandler(IApplicationDbContext dbContext)
	{
		_dbContext = dbContext;
	}

	public async Task<Unit> Handle(RemovePlacesFromPlan request, CancellationToken cancellationToken)
	{
		var plan = await _dbContext.Plans.Include(x => x.Places)
				.SingleOrDefaultAsync(x => x.Id == request.PlanId) ??
			throw new PlanNotFoundException(request.PlanId);
		var places = await _dbContext.Places.Where(x => request.Places.Contains(x.Id))
				.ToListAsync() ??
			throw new PlaceNotFoundException();

		plan.RemovePlaces(places);

		_dbContext.Plans.Update(plan);
		await _dbContext.SaveChangesAsync();

		return Unit.Value;
	}
}

public class RemovePlacesFromPlanValidator : AbstractValidator<RemovePlacesFromPlan>
{
	public RemovePlacesFromPlanValidator()
	{
		RuleFor(x => x.PlanId)
			.NotEmpty();
		RuleFor(x => x.Places)
			.NotEmpty();
	}
}