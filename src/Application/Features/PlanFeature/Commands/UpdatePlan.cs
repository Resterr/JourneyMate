using FluentValidation;
using JourneyMate.Application.Common.Exceptions;
using JourneyMate.Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace JourneyMate.Application.Features.PlanFeature.Commands;

public record UpdatePlan(Guid Id, string Name) : IRequest<Unit>;

internal sealed class UpdatePlanHandler : IRequestHandler<UpdatePlan, Unit>
{
	private readonly IApplicationDbContext _dbContext;

	public UpdatePlanHandler(IApplicationDbContext dbContext)
	{
		_dbContext = dbContext;
	}

	public async Task<Unit> Handle(UpdatePlan request, CancellationToken cancellationToken)
	{
		var plan = await _dbContext.Plans.SingleOrDefaultAsync(x => x.Id == request.Id) ?? throw new PlanNotFoundException(request.Id);
		plan.Update(request.Name);
		
		_dbContext.Plans.Update(plan);
		await _dbContext.SaveChangesAsync();

		return Unit.Value;
	}
}

public class UpdatePlanValidator : AbstractValidator<UpdatePlan>
{
	public UpdatePlanValidator()
	{
		RuleFor(x => x.Id)
			.NotEmpty();
		RuleFor(x => x.Name)
			.NotEmpty();
	}
}