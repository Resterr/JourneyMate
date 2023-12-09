using FluentValidation;
using JourneyMate.Application.Common.Exceptions;
using JourneyMate.Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace JourneyMate.Application.Features.PlanFeature.Commands;

public record RemovePlan(Guid Id) : IRequest<Unit>;

internal sealed class RemovePlanHandler : IRequestHandler<RemovePlan, Unit>
{
	private readonly IApplicationDbContext _dbContext;

	public RemovePlanHandler(IApplicationDbContext dbContext)
	{
		_dbContext = dbContext;
	}

	public async Task<Unit> Handle(RemovePlan request, CancellationToken cancellationToken)
	{
		var plan = await _dbContext.Plans.SingleOrDefaultAsync(x => x.Id == request.Id) ?? throw new PlanNotFound(request.Id);
		
		_dbContext.Plans.Remove(plan);
		await _dbContext.SaveChangesAsync();
		
		return Unit.Value;
	}
}

public class RemovePlanValidator : AbstractValidator<RemovePlan>
{
	public RemovePlanValidator()
	{
		RuleFor(x => x.Id)
			.NotEmpty();
	}
}