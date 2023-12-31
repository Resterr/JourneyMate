using FluentValidation;
using JourneyMate.Application.Common.Exceptions;
using JourneyMate.Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace JourneyMate.Application.Features.PlanFeature.Commands;

public record RemovePlaceFromPlan(Guid PlanId, Guid PlaceId) : IRequest<Unit>;

internal sealed class RemovePlaceFromPlanHandler : IRequestHandler<RemovePlaceFromPlan, Unit>
{
	private readonly IApplicationDbContext _dbContext;

	public RemovePlaceFromPlanHandler(IApplicationDbContext dbContext)
	{
		_dbContext = dbContext;
	}

	public async Task<Unit> Handle(RemovePlaceFromPlan request, CancellationToken cancellationToken)
	{
		var plan = await _dbContext.Plans.Include(x => x.Places)
				.SingleOrDefaultAsync(x => x.Id == request.PlanId) ??
			throw new PlanNotFoundException(request.PlanId);
		var place = await _dbContext.Places.SingleOrDefaultAsync(x => x.Id == request.PlaceId) ?? throw new PlaceNotFoundException(request.PlaceId);

		plan.RemovePlace(place);

		_dbContext.Plans.Update(plan);
		await _dbContext.SaveChangesAsync();

		return Unit.Value;
	}
}

public class RemovePlaceFromPlanValidator : AbstractValidator<RemovePlaceFromPlan>
{
	public RemovePlaceFromPlanValidator()
	{
		RuleFor(x => x.PlanId)
			.NotEmpty();
		RuleFor(x => x.PlaceId)
			.NotEmpty();
	}
}