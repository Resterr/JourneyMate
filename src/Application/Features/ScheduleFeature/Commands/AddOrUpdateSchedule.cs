using FluentValidation;
using JourneyMate.Application.Common.Exceptions;
using JourneyMate.Application.Common.Interfaces;
using JourneyMate.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace JourneyMate.Application.Features.ScheduleFeature.Commands;

public record AddOrUpdateSchedule(Guid PlanId, Guid PlaceId, DateTime StartingDate, DateTime? EndingDate) : IRequest<Unit>;

internal sealed class UpdateScheduleHandler : IRequestHandler<AddOrUpdateSchedule, Unit>
{
	private readonly IApplicationDbContext _dbContext;

	public UpdateScheduleHandler(IApplicationDbContext dbContext)
	{
		_dbContext = dbContext;
	}

	public async Task<Unit> Handle(AddOrUpdateSchedule request, CancellationToken cancellationToken)
	{
		var schedule = await _dbContext.Schedules.SingleOrDefaultAsync(x => x.PlanId == request.PlanId && x.PlaceId == request.PlaceId);

		if (schedule == null)
		{
			var plan = await _dbContext.Plans.SingleOrDefaultAsync(x => x.Id == request.PlanId) ?? throw new PlanNotFoundException(request.PlaceId);
			var place = await _dbContext.Places.SingleOrDefaultAsync(x => x.Id == request.PlaceId) ?? throw new PlaceNotFoundException(request.PlaceId);;
			schedule = new Schedule(plan, place, request.StartingDate, request.EndingDate);
			await _dbContext.Schedules.AddAsync(schedule);
			await _dbContext.SaveChangesAsync();
		}
		else
		{
			schedule.Update(request.StartingDate, request.EndingDate);
			_dbContext.Schedules.Update(schedule);
			await _dbContext.SaveChangesAsync();
		}
		
		return Unit.Value;
	}
}

public class UpdateScheduleValidator : AbstractValidator<AddOrUpdateSchedule>
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