using FluentValidation;
using JourneyMate.Application.Common.Exceptions;
using JourneyMate.Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace JourneyMate.Application.Features.PlanFeature.Commands;

public record UpdateSchedule(Guid PlanId, Guid PlaceId, DateTime StartingDate, DateTime? EndingDate) : IRequest<Unit>;

internal sealed class UpdateScheduleHandler : IRequestHandler<UpdateSchedule, Unit>
{
	private readonly IApplicationDbContext _dbContext;

	public UpdateScheduleHandler(IApplicationDbContext dbContext)
	{
		_dbContext = dbContext;
	}

	public async Task<Unit> Handle(UpdateSchedule request, CancellationToken cancellationToken)
	{
		var placePlan = await _dbContext.PlacePlans.SingleOrDefaultAsync(x => x.PlanId == request.PlanId && x.PlaceId == request.PlaceId) ?? throw new PlanNotFoundException(request.PlaceId);
		placePlan.UpdateSchedule(request.StartingDate, request.EndingDate);
		
		_dbContext.PlacePlans.Update(placePlan);
		await _dbContext.SaveChangesAsync();

		return Unit.Value;
	}
}

public class UpdateScheduleValidator : AbstractValidator<UpdateSchedule>
{
	public UpdateScheduleValidator()
	{
		RuleFor(x => x.PlanId)
			.NotEmpty();
		RuleFor(x => x.PlaceId)
			.NotEmpty();
		RuleFor(x => x.StartingDate)
			.NotEmpty();
		RuleFor(x => x.EndingDate)
			.GreaterThanOrEqualTo(x => x.StartingDate)
			.When(x => x.EndingDate != null);
	}
}